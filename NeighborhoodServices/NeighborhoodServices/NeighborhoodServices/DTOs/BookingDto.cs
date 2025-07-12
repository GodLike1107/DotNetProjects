namespace NeighborhoodServices.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public string ServiceTitle { get; set; }
        public string ProviderName { get; set; }
        public DateTimeOffset ScheduledTime { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
    }

}
