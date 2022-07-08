namespace Application.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Application.DTO;
    using Domain.Model.Auxiliar;

    public interface IItemService
    {
        Task<ICollection<ItemDto>> GetAllItemsAsync();

        Task<ItemDto> AddItemAsync(ItemDto itemDto);

        Task<ShoppingCostResult> AddToBasketAsync(List<string> inputItems);
    }
}