namespace Data.Repository.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Model;

    public interface IItemRepository
    {
        Task<Item> GetAllItemsAsync();

        Task<Item> AddItemsAsync(ICollection<Item> items);
    }
}