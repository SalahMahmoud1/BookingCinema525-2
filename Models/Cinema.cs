namespace BookingCinema525.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Img { get; set; } = "default.png";
        public bool Status { get; set; }
    }
}
