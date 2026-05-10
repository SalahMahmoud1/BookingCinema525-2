using Microsoft.AspNetCore.Identity;

namespace BookingCinema525_new.Utilities.DBSeeding
{
    public interface IDbInitializer
    {
        Task InitializeAsync();
    }
}
