namespace Application.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.DTO;
    using Application.Services.Interfaces;
    using Application.Services.Mappers;
    using Data.Repository.Interfaces;
    using Domain.Model.Auxiliar;

    public class ItemService : IItemService
    {
        private readonly IItemRepository itemsRepository;

        public ItemService(IItemRepository itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        public async Task<ICollection<ItemDto>> GetAllItemsAsync()
        {
            var items = await this.itemsRepository.GetAllItemsAsync();

            return items
              .Select(x => x.ToDto())
              .ToList();
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

        public async Task<ShoppingCostResult> AddToBasketAsync(List<string> inputItems)
        {
            if (inputItems is null || inputItems.Count == 0)
            {
                return null;
            }

            var items = await this.GetAllItemsAsync();

            if (items is null || items.Count == 0)
            {
                return null;
            }

            var itemsData = items.Where(x => inputItems.Any(b => b == x.Name)).ToList();

            if (itemsData.Count() == 0)
            {
                var names = string.Join(", ", inputItems.Select(x => x));

                return new ShoppingCostResult
                {
                    Success = false,
                    Message = $"Whoops! Something went wrong, can't find item with the name(s): {names}",
                };
            }

            inputItems.RemoveAll(x => !itemsData.Exists(b => b.Name.ToLower() == x.ToLower()));

            var priceItems = inputItems
                .GroupBy(itemName => itemName)
                //Count Item quantities
                .Select(g =>
                {
                    return new
                    {
                        Name = g.Key,
                        Quantity = g.Count(),
                    };
                })
                .GroupBy(i => (ItemName: i.Name.ToLower(), Quantity: i.Quantity))
                //Calculate Total Price per Item
                .Select(g =>
                {
                    var itemPrice = itemsData.Where(x => x.Name.ToLower() == g.Key.ItemName).FirstOrDefault().Price;

                    var subTotal = g.Key.Quantity * itemPrice;
                    var total = subTotal;

                    //apply discounts

                    return new ShoppingCostResult
                    {
                        Success = true,
                        SubTotal = total,
                        Total = subTotal,
                    };
                }).ToList();

            var subTotal = priceItems.Sum(x => x.SubTotal);
            var total = priceItems.Sum(x => x.Total);

            return new ShoppingCostResult
            {
                Success = true,
                SubTotal = subTotal,
                Total = total,
            };
        }
    }
}