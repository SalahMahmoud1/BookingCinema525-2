using BookingCinema525.Repositories;
using BookingCinema525_new.Models;
using Ecommerce525.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BookingCinema525
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            var connectionString =
                    builder.Configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string"
                    + "'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(connectionString);
            });
            builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options=>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;

            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            builder.Services.AddScoped<IRepository<Movie>, Repository<Movie>>();
            builder.Services.AddScoped < IRepository <Category>, Repository<Category>>();
            builder.Services.AddScoped<IRepository<ApplicationUserOTP>, Repository<ApplicationUserOTP>>();
            builder.Services.AddScoped<IRepository<Cinema>, Repository<Cinema>>();
            builder.Services.AddScoped<IRepository<Actor>, Repository<Actor>>();
            builder.Services.AddScoped<IMovieSubImageRepository, MovieSubImageRepository >();
            builder.Services.AddScoped<IActorMovieListRepository, ActorMovieListRepository>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(

        name: "default",
            //pattern: "{controller=Home}/{action=Index}/{id?}")
            //pattern: "{area=Main}/{controller=HomeMovie}/{action=Index}/{id?}")
            pattern: "{area=Identity}/{controller=Account}/{action=Login}/{id?}")
            //pattern: "{area=Main}/{controller=Home}/{action=Index}/{id?}")
            //pattern: "{area=Admin}/{controller=Movie}/{action=Index}/{id?}")
            .WithStaticAssets();
            app.Run();
        }
    }
}
