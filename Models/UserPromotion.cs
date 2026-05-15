using Microsoft.EntityFrameworkCore;

namespace BookingCinema525_new.Models
{
    [PrimaryKey(nameof(PromotionId), nameof(ApplicationUserId))]
    public class UserPromotion
    {
        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
