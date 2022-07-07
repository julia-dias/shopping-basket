namespace Data.Repository.Implementations
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Repository.Interfaces;
    using Domain.Model;

    public class ItemRepository : IItemRepository
    {
        public Task<Item> GetAllItemsAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Item> AddItemsAsync(ICollection<Item> items)
        {
            throw new System.NotImplementedException();
        }
    }
}