namespace Domain.Model
{
    public class ItemDiscountFromMultiBuy : ItemDiscount
    {
        public int Quantity { get; set; }

        public override decimal CalculateDiscount(int quantity, decimal itemPrice)
        {
            var quantityForDiscount = quantity > this.Quantity
                ? this.Quantity
                : quantity;

            return quantityForDiscount * itemPrice * this.DiscountPercentage / 100;
        }
    }
}