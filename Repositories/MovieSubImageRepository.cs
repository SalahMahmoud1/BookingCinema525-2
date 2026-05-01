namespace BookingCinema525.Repositories
{
    public class MovieSubImageRepository:Repository<MovieSubImage>, IMovieSubImageRepository
    {
        public MovieSubImageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void RemoveRange(IEnumerable<MovieSubImage> movieSubImages)
        {
            _context.MovieSubImages.RemoveRange(movieSubImages);
        }
    }
}
