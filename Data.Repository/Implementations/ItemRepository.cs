namespace Data.Repository.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Repository.Interfaces;
    using Domain.Model;

    public class ItemRepository : IItemRepository
    {
        private static ICollection<Item> itemsInMemory;

        public ItemRepository()
        {
            itemsInMemory = new List<Item>();
        }

        public Task<ICollection<Item>> GetAllItemsAsync()
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
    }
}