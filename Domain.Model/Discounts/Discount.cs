namespace Domain.Model
{
    using System;

    public abstract class Discount
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public string ItemReference { get; set; }

        public bool IsActive { get; set; }

        public abstract decimal CalculateDiscount(int quantity, decimal itemPrice);
    }
}