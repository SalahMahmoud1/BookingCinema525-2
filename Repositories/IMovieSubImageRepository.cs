namespace BookingCinema525.Repositories
{
    public interface IMovieSubImageRepository:IRepository<MovieSubImage>
    {
        void RemoveRange(IEnumerable<MovieSubImage> movieSubImages);
    }
}
