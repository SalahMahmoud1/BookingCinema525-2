using BookingCinema525.DataAccess;
using BookingCinema525.Repositories;
using BookingCinema525.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingCinema525.Areas.Main.Controllers
{
    [Area("Main")]
    public class HomeMovieController : Controller
    {
        ApplicationDbContext _context; //= new ApplicationDbContext();
        private readonly IRepository<Movie> _movieRepository;
        public HomeMovieController(ApplicationDbContext context, IRepository<Movie> movieRepository)
        {
            _context = context;
            _movieRepository = movieRepository;
        }

        public IActionResult Index(FilterMovieVm filter)
        {
            var movies = _context.Movies.AsQueryable();
            movies = movies.Include(m => m.Category);
            if (filter.MovieName is not null)
            {
                movies = movies.Where(m => m.Name.Contains(filter.MovieName));
                ViewBag.MovieName = filter.MovieName;
            }
            if (filter.MinPrice > 0)
            {
                movies = movies.Where(m => m.Price >= filter.MinPrice);
                ViewBag.MinPrice = filter.MinPrice;
            }
            if (filter.MaxPrice > 0)
            {
                movies = movies.Where(m => m.Price  <= filter.MaxPrice);
                ViewBag.MaxPrice = filter.MaxPrice;
            }
            if (filter.CategoryId > 0)
            {
                movies = movies.Where(m => m.CategoryId == filter.CategoryId);
                ViewBag.CategoryId = filter.CategoryId;
            }
            if (filter.CinemaId > 0)
            {
                movies = movies.Where(p => p.CinemaId == filter.CinemaId);
                ViewBag.CinemaId = filter.CinemaId;
            }

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Cinemas = _context.Cinemas.ToList();
            ViewBag.TotalPages = (int)Math.Ceiling(movies.Count() / 8.0);
            ViewBag.CurrentPage = filter.Page;
            movies = movies.Skip((filter.Page - 1) * 8).Take(8);
            return View(movies.ToList());
        }
        public async Task<IActionResult> MovieDetails(int id)
        {
            var movie = await _movieRepository.GetOneAsync(m => m.Id == id, includes: [m=>m.Category]);
            if (movie == null) return NotFound();
            var relatedMovies =await _movieRepository.GetAsync(m => m.CategoryId == movie.CategoryId && m.Id != movie.Id, includes: [m => m.Category]);
            relatedMovies = relatedMovies.Skip(0).Take(4);
            return View(new MovieWithRelatedVM
            {
                 Movie = movie,
                 RelatedMovies = relatedMovies
            });

        }
    }
}
