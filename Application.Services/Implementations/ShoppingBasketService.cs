namespace Application.Services.Implementations
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Application.Services.Interfaces;
    using Domain.Model.Auxiliar;

    public class ShoppingBasketService : IShoppingBasketService
    {
        public Task<ShoppingCostResult> AddToBasketAsync(ICollection<string> items)
        {
            throw new System.NotImplementedException();
        }
    }
}