namespace Application.Services.Mappers
{
    using System;
    using Application.DTO;
    using Domain.Model;

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
                Id = item.Id,
                Name = item.Name,
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
                Id = item.Id,
                Name = item.Name,
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