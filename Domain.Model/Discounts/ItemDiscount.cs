namespace Domain.Model
{
    public class ItemDiscount : Discount
    {
        public decimal DiscountPercentage { get; set; }

        public override decimal CalculateDiscount(int quantity, decimal itemPrice)
        {
            return quantity * itemPrice * this.DiscountPercentage / 100;
        }
    }
}