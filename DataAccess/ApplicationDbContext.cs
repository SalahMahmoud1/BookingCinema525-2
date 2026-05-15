using BookingCinema525.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookingCinema525_new.ViewModels;
using BookingCinema525_new.Models;

namespace BookingCinema525.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieSubImage> MovieSubImages{ get; set; }
        public DbSet<ActorMovieList> ActorMovieLists {  get; set; }
        public DbSet<ApplicationUserOTP> ApplicationUserOTPs { get; set; }
        public DbSet<BookingCinema525_new.ViewModels.VerifyOTPVM> VerifyOTPVM { get; set; } = default!;
        public DbSet<BookingCinema525_new.ViewModels.ResetPasswordVM> ResetPasswordVM { get; set; } = default!;
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<UserPromotion> UserPromotions { get; set; }


        //public DbSet<BookingCinema525_new.ViewModels.RegisterVM> RegisterVM { get; set; } = default!;
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog = BookingCinemaDB ;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;");
        //}
    }
}
