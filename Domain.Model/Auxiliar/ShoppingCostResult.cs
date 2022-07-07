namespace Domain.Model.Auxiliar
{
    using System.Collections.Generic;

    public class ShoppingCostResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public decimal? SubTotal { get; set; }

        public decimal? Total { get; set; }

        public List<DiscountItemsResult> DiscountItems { get; set; }
    }
}