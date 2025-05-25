namespace BookingApi.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int PropertyId { get; set; }
        public int BookBy { get; set; }
        public DateTime BookOn { get; set; } = DateTime.UtcNow;
    }
}
