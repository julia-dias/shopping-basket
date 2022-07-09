namespace Data.Repository.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Model;

    public interface IItemRepository
    {
        Task<IList<Item>> GetAllItemsAsync();

        Task<Item> AddItemAsync(Item item);

        Task<IEnumerable<Discount>> GetDiscounts(string[] itemReferences);
    }
}