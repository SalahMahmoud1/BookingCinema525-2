using Microsoft.EntityFrameworkCore;

namespace BookingCinema525.Models
{
    [PrimaryKey(nameof(ActorId),nameof(MovieID))]
    public class ActorMovieList
    {
        public int ActorId {  get; set; }
        public Actor Actor { get; set; }
        public int MovieID {  get; set; }
        public Movie Movie { get; set; }
    }
}
