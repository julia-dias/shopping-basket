namespace Domain.Model
{
    using System;

    public class Item
    {
        public int Id { get; set; }

        public string Reference { get; set; }

        public decimal Price { get; set; }

        public PriceUnitEnum PriceUnit { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}