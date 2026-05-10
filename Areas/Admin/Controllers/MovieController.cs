using BookingCinema525.Repositories;
using BookingCinema525_new.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace BookingCinema525.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize()]
    public class MovieController : Controller
    {
        //ApplicationDbContext _context = new ApplicationDbContext();
        IRepository<Movie> _movieRepository;// = new Repository<Movie>();
        IRepository<Category> _categoryRepository;// = new Repository<Category>();
        IRepository<Cinema> _cinemaRepository;// = new Repository<Cinema>();
        IMovieSubImageRepository _movieSubImageRepository;// = new MovieSubImageRepository();
        IActorMovieListRepository _actorMovieListRepository;// = new ActorMovieListRepository();
        IRepository<Actor> _actorRepository;//= new Repository<Actor>();

        public MovieController(IRepository<Movie> movieRepository, IRepository<Category> categoryRepository, IRepository<Cinema> cinemaRepository, IMovieSubImageRepository movieSubImageRepository, IActorMovieListRepository actorMovieListRepository, IRepository<Actor> actorRepository)
        {
            _movieRepository = movieRepository;
            _categoryRepository = categoryRepository;
            _cinemaRepository = cinemaRepository;
            _movieSubImageRepository = movieSubImageRepository;
            _actorMovieListRepository = actorMovieListRepository;
            _actorRepository = actorRepository;
        }

        public async Task<IActionResult> Index(FilterMovieVm filter)
        {
            //var movies = _context.Movies.AsQueryable();
            //movies = movies.Include(m => m.Category);
            var movies =await _movieRepository.GetAsync(includes: [c => c.Category, b => b.Cinema]);//movies.Include(m => m.Category);
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
                movies = movies.Where(m => m.Price <= filter.MaxPrice);
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

            //ViewBag.Categories = _context.Categories.ToList();
            //ViewBag.Cinemas = _context.Cinemas.ToList();
            ViewBag.Categories =await _categoryRepository.GetAsync();
            ViewBag.Cinemas =await _cinemaRepository.GetAsync();
            ViewBag.TotalPages = (int)Math.Ceiling(movies.Count() / 8.0);
            ViewBag.CurrentPage = filter.Page;
            movies = movies.Skip((filter.Page - 1) * 8).Take(8);
            return View(movies.ToList());
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //var categories = _context.Categories.ToList();
            //var cinemas = _context.Cinemas.ToList();
            //var actor = _context.Actors.ToList();
            var categories =await _categoryRepository.GetAsync();
            var cinemas =await _cinemaRepository.GetAsync();
            var actor =await _actorRepository.GetAsync();
            return View(new MovieVm()
            {
                Categories = categories.ToList(),
                Cinemas = cinemas.ToList(), 
                Actors = actor.ToList()
                
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(Movie movie, IFormFile ImgFile, List<IFormFile> SubImgFiles, List<int> ActorList)
        {
            if (ImgFile != null && ImgFile.Length > 0)
            {
                //var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgFile.FileName);
                var fileName = Guid.NewGuid().ToString() + "-" + ImgFile.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\MovieImages", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    ImgFile.CopyTo(stream);
                }
                movie.MainImg = fileName;
            }
            //var SavedMovie = _context.Movies.Add(movie);
            //_context.SaveChanges();
            var SavedMovie =await _movieRepository.CreateAsync(movie);
            await _movieRepository.CommitAsync();
            if (SubImgFiles != null && SubImgFiles.Count > 0)
            {
                foreach (var image in SubImgFiles)
                {
                    if (image != null && image.Length > 0)
                    {
                        //var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgFile.FileName);
                        var fileName = Guid.NewGuid().ToString() + "-" + image.FileName;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\MovieImages\\MovieSubImages", fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            image.CopyTo(stream);
                        }
                        //_context.MovieSubImages.Add(new MovieSubImage()
                        await _movieSubImageRepository.CreateAsync(new MovieSubImage()
                        {
                            MovieId = SavedMovie.Entity.Id,
                            Img = fileName,
                        });

                    }
                }
                //_context.SaveChanges();
                await _movieSubImageRepository.CommitAsync();
            }
            if (ActorList != null && ActorList.Count > 0)
            {
                foreach (var actor in ActorList)
                {
                    //_context.ActorMovieLists.Add(new ActorMovieList()
                    await _actorMovieListRepository.CreateAsync(new ActorMovieList()
                    {
                        MovieID = SavedMovie.Entity.Id,
                        ActorId = actor
                    });
                }
            }
            //_context.SaveChanges();
            await _actorMovieListRepository.CommitAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = $"{CD.SUPER_ADMIN_ROLE},{CD.ADMIN_ROLE}")]
        public async Task<IActionResult> Update(int id)
        {

            //var movie = _context.Movies.FirstOrDefault(c => c.Id == id);
            var movie =await _movieRepository.GetOneAsync(filter: m => m.Id == id);
            if (movie == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            return View(new MovieVm() 
            {
                //Categories = _context.Categories.ToList(),
                //Cinemas = _context.Cinemas.ToList(),
                //MovieSubImage = _context.MovieSubImages.Where(p => p.MovieId == id).ToList(),
                //ActorMovieLists = _context.ActorMovieLists.Where(p => p.MovieID == id).ToList(),
                //Movie = movie
                Categories =(await _categoryRepository.GetAsync()).ToList(),
                Cinemas = (await _cinemaRepository.GetAsync()).ToList(),
                MovieSubImage =(await _movieSubImageRepository.GetAsync(filter: p => p.MovieId == id)).ToList(),
                ActorMovieLists =(await _actorMovieListRepository.GetAsync(filter: p => p.MovieID == id)).ToList(),
                Movie = movie
            });
        }
        [HttpPost]
        [Authorize(Roles = $"{CD.SUPER_ADMIN_ROLE},{CD.ADMIN_ROLE}")]
        public async Task<IActionResult> Update(Movie movie, IFormFile ImgFile, List<IFormFile> SubImgFiles, List<int> ActorList)
        {
            //var movieInDb = _context.Movies.AsNoTracking().FirstOrDefault(m => m.Id == movie.Id);
            var movieInDb =await _movieRepository.GetOneAsync(filter: m => m.Id == movie.Id,tracked:false);

            if (ImgFile != null && ImgFile.Length > 0)
            {
                //var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgFile.FileName);
                var fileName = Guid.NewGuid().ToString() + "-" + ImgFile.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\MovieImages", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    ImgFile.CopyTo(stream);
                }
                movie.MainImg = fileName;
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\MovieImages", movieInDb.MainImg);

                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }
            else
            {
                movie.MainImg = movieInDb.MainImg;
            }
            _movieRepository.Update(movie);
            await _movieRepository.CommitAsync();
            if (SubImgFiles != null && SubImgFiles.Count > 0)
            {
                // remove from DB
                //var oldProductSubImages = _context.MovieSubImages.Where(p => p.MovieId == movie.Id);
                // remove Form wwwroot
                var oldMovieSubImages =await _movieSubImageRepository.GetAsync(filter: m => m.MovieId == movie.Id);
                if (oldMovieSubImages != null)
                {
                    //_context.MovieSubImages.RemoveRange(oldMovieSubImages);
                    _movieSubImageRepository.RemoveRange(oldMovieSubImages);
                    foreach (var item in oldMovieSubImages)
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\MovieImages\\ProductSubImages", item.Img);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                }
                foreach (var image in SubImgFiles)
                {
                    if (image != null && image.Length > 0)
                    {
                        // insert in wwwroot
                        //var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgFile.FileName);
                        var fileName = Guid.NewGuid().ToString() + "-" + image.FileName;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\MovieImages\\MovieSubImages", fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            image.CopyTo(stream);
                        }
                        // insert in DB
                        await _movieSubImageRepository.CreateAsync(new MovieSubImage()
                        {
                            MovieId = movie.Id,
                            Img = fileName,
                        });

                    }
                }
                //_context.SaveChanges();
                await _movieSubImageRepository.CommitAsync();
            }
            if (ActorList != null && ActorList.Count > 0)
            {
                // remove from DB
                var oldActorMovieList =await _actorMovieListRepository.GetAsync(p => p.MovieID == movie.Id);
                if (oldActorMovieList is not null)
                {
                    //_context.ActorMovieLists.RemoveRange(oldMovieSubImages);
                    _actorMovieListRepository.RemoveRange(oldActorMovieList);
                }
                foreach (var actor in ActorList)
                {
                    // _context.ActorMovieLists.Add(new ActorMovieList()
                    await _actorMovieListRepository.CreateAsync(new ActorMovieList()
                    {
                        MovieID = movie.Id,
                        ActorId = actor,
                    });
                }
            }
            //_context.SaveChanges();
            await _actorMovieListRepository.CommitAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = $"{CD.SUPER_ADMIN_ROLE},{CD.ADMIN_ROLE}")]
        public async Task<IActionResult> Delete(int id)
        {
            //var movie = _context.Movies.FirstOrDefault(c => c.Id == id);
            var movie =await _movieRepository.GetOneAsync(filter: c => c.Id == id);
            if (movie == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\MovieImages", movie.MainImg);

            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }
            var oldMovieSubImages =await _movieSubImageRepository.GetAsync(filter: p => p.MovieId == movie.Id);

            foreach (var item in oldMovieSubImages)
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\MovieImages\\ProductSubImages", item.Img);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            _movieRepository.Delete(movie);
            await _movieRepository.CommitAsync();
            return RedirectToAction(nameof(Index));

        }

    }
}
