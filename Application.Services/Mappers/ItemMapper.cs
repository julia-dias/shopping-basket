namespace Application.Services.Mappers
{
    using System;
    using Application.DTO;
    using Domain.Model;
    using Domain.Model.Item;

    public static class ItemMapper
    {
        public static ItemDto ToDto(this Item item)
        {
            if (item is null)
            {
                return null;
            }

            return new ItemDto
            {
                Reference = item.Reference,
                Price = item.Price,
                PriceUnit = item.PriceUnit.ToString(),
            };
        }

        public static Item ToDomain(this ItemDto item)
        {
            if (!ValidateDto(item))
            {
                return null;
            }

            return new Item
            {
                Reference = item.Reference,
                Price = item.Price,
                PriceUnit = Enum.Parse<PriceUnitEnum>(item.PriceUnit),
            };
        }

        private static bool ValidateDto(this ItemDto item)
        {
            return !(item is null || !Enum.IsDefined(typeof(PriceUnitEnum), item.PriceUnit));
        }
    }
}