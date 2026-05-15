using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingCinema525_new.Models
{
    [PrimaryKey(nameof(MovieId), nameof(ApplicationUserId))]
    public class Cart
    {
        public int MovieId { get; set; }
        [ForeignKey(nameof(MovieId))]
        public Movie Movie { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
