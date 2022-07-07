namespace Domain.Model.Auxiliar
{
    public class DiscountItemsResult
    {
        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public int DiscountPrice { get; set; }

        public decimal DiscountPercentage { get; set; }
    }
}