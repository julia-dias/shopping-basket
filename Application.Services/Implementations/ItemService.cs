namespace Application.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.DTO;
    using Application.Services.Interfaces;
    using Application.Services.Mappers;
    using Data.Repository.Interfaces;
    using Domain.Model;
    using Domain.Model.Auxiliar;

    public class ItemService : IItemService
    {
        private readonly IItemRepository itemsRepository;

        public ItemService(IItemRepository itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        public async Task<IList<ItemDto>> GetAllItemsAsync()
        {
            var items = await this.itemsRepository.GetAllItemsAsync();

            return items.Select(x => x.ToDto()).ToList();
        }

        public async Task<ItemDto> AddItemAsync(ItemDto itemDto)
        {
            if (itemDto is null)
            {
                return null;
            }

            var item = itemDto.ToDomain();

            var newItem = await this.itemsRepository.AddItemAsync(item);

            return newItem.ToDto();
        }

        public async Task<ShoppingCostResult> AddToBasketAsync(List<string> itemsInput)
        {
            if (itemsInput is null || itemsInput.Count == 0)
            {
                return null;
            }

            var itemsMetadata = await this.GetItemsData(itemsInput);

            if (itemsMetadata.Count() == 0)
            {
                return this.ThrowError(itemsInput);
            }

            return await this.CalculateShoppingCostResult(itemsInput, itemsMetadata);
        }

        private async Task<List<ItemDto>> GetItemsData(List<string> inputItems)
        {
            var items = await this.GetAllItemsAsync();

            if (items is null || items.Count == 0)
            {
                return null;
            }

            return items.Where(x => inputItems.Any(b => b == x.Reference)).ToList();
        }

        private ShoppingCostResult ThrowError(List<string> inputItems)
        {
            var names = string.Join(", ", inputItems.Select(x => x));

            return new ShoppingCostResult
            {
                Success = false,
                Message = $"Whoops! Something went wrong, can't find item with the name(s): {names}",
            };
        }

        private async Task<ShoppingCostResult> CalculateShoppingCostResult(
            List<string> itemsInput,
            List<ItemDto> itemsMetadata)
        {
            itemsInput.RemoveAll(x => !itemsMetadata.Exists(b => b.Reference.ToLower() == x.ToLower()));

            var countItems = this.CountItems(itemsInput);

            var itemsDiscounts = await this.GetItemsDiscounts(countItems);

            var discountFinalResult = new List<DiscountItemsResult>();

            var priceItems = countItems
                .GroupBy(i => (ItemName: i.Name.ToLower(), Quantity: i.Quantity))
                //Calculate Total Price per Item
                .Select(g =>
                {
                    var item = itemsMetadata.Where(x => x.Reference.ToLower() == g.Key.ItemName).FirstOrDefault();

                    var subTotal = g.Key.Quantity * item.Price;

                    var discountData = itemsDiscounts.Where(x => x.ItemReference == item.Reference).ToList();

                    //apply discounts
                    decimal discountPrice = 0;

                    if (discountData != null && discountData.Count() > 0)
                    {
                        foreach (ItemDiscount discount in discountData)
                        {
                            discountPrice = discount.CalculateDiscount(g.Key.Quantity, item.Price);

                            var discoutResult = new DiscountItemsResult
                            {
                                ItemReference = item.Reference,
                                DiscountPrice = discountPrice,
                                DiscountPercentage = discount.DiscountPercentage,
                            };

                            discountFinalResult.Add(discoutResult);
                        }
                    }

                    var total = subTotal - discountPrice;

                    return new ShoppingCostResult
                    {
                        Success = true,
                        SubTotal = subTotal,
                        Total = total,
                    };
                }).ToList();

            var subTotal = priceItems.Sum(x => x.SubTotal);
            var total = priceItems.Sum(x => x.Total);

            return new ShoppingCostResult
            {
                Success = true,
                SubTotal = subTotal,
                Total = total,
                DiscountItems = discountFinalResult,
            };
        }

        private async Task<List<Discount>> GetItemsDiscounts(List<ItemsQuantity> itemsCount)
        {
            var discounts = await this.itemsRepository.GetDiscounts(itemsCount.Select(x => x.Name).ToArray());

            var multibuyDiscounts = discounts.Where(x => x.GetType() == typeof(MultibuyDiscount)).ToList();

            var itemsDiscounts = discounts.Where(x => x.GetType() == typeof(ItemDiscount) && x.IsActive).ToList();

            foreach (MultibuyDiscount discount in multibuyDiscounts)
            {
                var item = itemsCount.Where(x => x.Name == discount.ItemReference).FirstOrDefault();

                if (item?.Quantity >= discount.ItemQuantity)
                {
                    decimal discountQuantity = item.Quantity / discount.ItemQuantity;
                    var finalDiscountQuantity = (int)Math.Floor(discountQuantity);

                    itemsDiscounts.Add(new ItemDiscountFromMultiBuy
                    {
                        Id = 0,
                        ItemId = discount.ItemOfferId,
                        ItemReference = discount.ItemOfferReference,
                        DiscountPercentage = discount.ItemOfferDiscountPercentage,
                        Quantity = discount.ItemOfferQuantity * finalDiscountQuantity,
                        IsActive = true,
                    });
                }
            }

            return itemsDiscounts;
        }

        private List<ItemsQuantity> CountItems(List<string> itemsInput)
        {
            var countItems = itemsInput
                .GroupBy(itemName => itemName)
                //Count Item quantities
                .Select(g =>
                {
                    return new ItemsQuantity
                    {
                        Name = g.Key,
                        Quantity = g.Count(),
                    };
                }).ToList();

            return countItems;
        }
    }
}