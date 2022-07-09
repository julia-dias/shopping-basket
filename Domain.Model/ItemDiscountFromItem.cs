namespace Domain.Model
{
    public class ItemDiscountFromItem : ItemDiscount
    {
        public int Quantity { get; set; }

        public override decimal CalculateDiscount(int quantity, decimal itemPrice)
        {
            return quantity * itemPrice * this.DiscountPercentage / 100;
        }
    }
}