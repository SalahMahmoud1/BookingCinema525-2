namespace BookingCinema525.Repositories
{
    public class ActorMovieListRepository:Repository<ActorMovieList>, IActorMovieListRepository
    {
        public ActorMovieListRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void RemoveRange(IEnumerable<ActorMovieList> actorMovieList)
        {
            _context.ActorMovieLists.RemoveRange(actorMovieList);
        }
    }
}
