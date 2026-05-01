using Microsoft.EntityFrameworkCore;

namespace BookingCinema525.Repositories
{
    public interface IActorMovieListRepository:IRepository<ActorMovieList>
    {
        void RemoveRange(IEnumerable<ActorMovieList> actorMovieList);
    }
}
