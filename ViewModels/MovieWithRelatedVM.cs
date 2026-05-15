namespace BookingCinema525_new.ViewModels
{
    public class MovieWithRelatedVM
    {
        public Movie Movie { get; set; }
        public IEnumerable<Movie> RelatedMovies { get; set; }

    }
}
