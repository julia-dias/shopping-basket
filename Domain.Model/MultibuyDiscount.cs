namespace Domain.Model
{
    public class MultibuyDiscount : Discount
    {
        public int ItemQuantity { get; set; }

        public int ItemOfferId { get; set; }

        public string ItemOfferReference { get; set; }

        public int ItemOfferQuantity { get; set; }

        public decimal ItemOfferDiscountPercentage { get; set; }

        public override decimal CalculateDiscount(int quantity, decimal itemPrice)
        {
            return default;
        }
    }
}