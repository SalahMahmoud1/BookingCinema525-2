using BookingCinema525.Models;

namespace BookingCinema525.ViewModels
{
    public class MovieVm
    {
        public List<Category> Categories { get; set; }
        public List<Cinema> Cinemas { get; set; }
        public List<Actor> Actors { get; set; }
        public List<MovieSubImage>? MovieSubImage { get; set; }
        public List<ActorMovieList>? ActorMovieLists { get; set; }
        public Movie? Movie { get; set; }
    }
}
