using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingCinema525_new.Models
{
    [Index(nameof(Code), IsUnique = true)]
    public class Promotion
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int MaxUsage { get; set; }
        public decimal Discount { get; set; }
        public bool IsValid { get; set; }
        public DateTime ValidTo { get; set; }
        public int MovieId { get; set; }
        [ForeignKey(nameof(MovieId))]
        public Movie Movie { get; set; }
    }
}
