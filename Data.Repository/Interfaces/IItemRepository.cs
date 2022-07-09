namespace Data.Repository.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Model;
    using Domain.Model.Item;

    public interface IItemRepository
    {
        Task<IList<Item>> GetAllItemsAsync();

        Task<Item> AddItemAsync(Item item);

        Task<IEnumerable<Discount>> GetDiscountsAsync(string[] itemReferences);
    }
}