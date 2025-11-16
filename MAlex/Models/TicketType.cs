namespace MAlex.Models
{
    public class TicketType
    {
        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
    }
}
