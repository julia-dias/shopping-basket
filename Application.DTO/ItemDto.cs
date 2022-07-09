namespace Application.DTO
{
    public class ItemDto
    {
        public int Id { get; set; }

        public string Reference { get; set; }

        public decimal Price { get; set; }

        public string PriceUnit { get; set; }

        public bool HasDiscount { get; set; }

        public int? DiscountId { get; set; }
    }
}