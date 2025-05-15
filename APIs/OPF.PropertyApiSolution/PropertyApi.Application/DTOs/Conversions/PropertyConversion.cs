using PropertyApi.Domain.Entities;

namespace PropertyApi.Application.DTOs.Conversions
{
    public static class PropertyConversion
    {
        public static Property ToEntity(PropertyDTO property) => new()
        {
            Id = property.Id,
            Name = property.Name,
            Description = property.Description,
            Area = property.Area,
            Location = property.Location,
            NumberOfRooms = property.NumberOfRooms,
            Price = property.Price,
        };

        public static (PropertyDTO?, IEnumerable<PropertyDTO>?) FromEntity(Property property, IEnumerable<Property>? properties)
        {
            //return single
            if (property is not null || properties is null)
            {
                var singlePropety = new PropertyDTO
                    (
                    property!.Id,
                    property.Name!,
                    property.Description!,
                    property.Area,
                    property.Location!,
                    property.NumberOfRooms,
                    property.Price
                    );
                return (singlePropety, null);
            }

            //return list
            if (properties is not null || property is null)
            {
                var _properties = properties!.Select(p =>
                    new PropertyDTO(p.Id, p.Name!, p.Description!, p.Area, p.Location!, p.NumberOfRooms, p.Price)).ToList();

                return (null, _properties);
            }

            return (null, null);
        }
    }
}
