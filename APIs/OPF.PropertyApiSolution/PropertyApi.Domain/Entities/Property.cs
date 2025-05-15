namespace PropertyApi.Domain.Entities
{
    public class Property
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Area { get; set; }
        public string? Location { get; set; }
        public int NumberOfRooms { get; set; }
        public decimal Price { get; set; }
    }
}
