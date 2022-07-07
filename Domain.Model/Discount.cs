namespace Domain.Model
{
    public abstract class Discount
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int ItemId { get; set; }

        public bool IsActive { get; set; }
    }
}