namespace BookingCinema525.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Rate { get; set; }
        public string MainImg { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public bool Status { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        
    }
}
