using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace BookingCinema525.Models
{
    [PrimaryKey(nameof(MovieId), nameof(Img))]
    public class MovieSubImage
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public string Img { get; set; }
    }
}

