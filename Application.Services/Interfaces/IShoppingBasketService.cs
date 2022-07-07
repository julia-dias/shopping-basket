namespace Application.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Model.Auxiliar;

    public interface IShoppingBasketService
    {
        Task<ShoppingCostResult> AddToBasketAsync(ICollection<string> items);
    }
}