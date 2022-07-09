namespace Data.Repository.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Repository.Interfaces;
    using Domain.Model;

    public class ItemRepository : IItemRepository
    {
        private static IList<Item> itemsInMemory;
        private static IList<Discount> discountsInMemory;

        public ItemRepository()
        {
            itemsInMemory = new List<Item>();
            discountsInMemory = new List<Discount>{
                new ItemDiscount {
                    Id = 1,
                    Description = "Apples have a 10% discount off their normal price",
                    ItemReference = "apples",
                    DiscountPercentage = 10,
                    IsActive = true,
                },
                new MultibuyDiscount {
                    Id = 2,
                    Description = "Buy 2 tins of soup and get a loaf of bread for half price",
                    ItemReference = "soup",
                    ItemQuantity = 2,
                    ItemOfferReference = "bread",
                    ItemOfferQuantity = 1,
                    ItemOfferDiscountPercentage = 50,
                    IsActive = true,
                },
            };
        }

        public Task<IList<Item>> GetAllItemsAsync()
        {
            return Task.FromResult(itemsInMemory);
        }

        public Task<Item> AddItemAsync(Item item)
        {
            item.DateCreated = DateTime.Now;
            item.DateUpdated = item.DateCreated;

            itemsInMemory.Add(item);

            return Task.FromResult(item);
        }

        public Task<IEnumerable<Discount>> GetDiscounts(string[] itemReferences)
        {
            return Task.FromResult(discountsInMemory.Where(x => itemReferences.Any(y => y == x.ItemReference)));
        }
    }
}