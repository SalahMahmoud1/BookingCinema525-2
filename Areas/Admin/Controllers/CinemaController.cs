using BookingCinema525.Repositories;
using BookingCinema525_new.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
namespace BookingCinema525.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize()]
    public class CinemaController : Controller
    {
        //ApplicationDbContext _context = new ApplicationDbContext();
        IRepository<Cinema> _cinemaRepository; //= new Repository<Cinema>();

        public CinemaController(IRepository<Cinema> cinemaRepository)
        {
            _cinemaRepository = cinemaRepository;
        }

        public async Task<IActionResult> Index()
        {
            //var cinemas = _context.Cinemas.AsQueryable();
            var cinemas =await _cinemaRepository.GetAsync();
            return View(cinemas.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Cinema());
        }
        [HttpPost]
        public async Task<IActionResult> Create(Cinema cinema, IFormFile ImgFile)
        {
            if (ImgFile != null && ImgFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "-" + ImgFile.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\CinemaImages", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    ImgFile.CopyTo(stream);
                }
                cinema.Img = fileName;
            }
            //_context.Cinemas.Add(cinema);
            //_context.SaveChanges();
            await _cinemaRepository.CreateAsync(cinema);
            await _cinemaRepository.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = $"{CD.SUPER_ADMIN_ROLE},{CD.ADMIN_ROLE}")]
        public async Task<IActionResult> Update(int id)
        {
            //var cinema = _context.Cinemas.FirstOrDefault(c => c.Id == id);
            var cinema =await _cinemaRepository.GetOneAsync(filter:c => c.Id == id);
            if (cinema == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            return View(cinema);
           
        }
        [HttpPost]
        [Authorize(Roles = $"{CD.SUPER_ADMIN_ROLE},{CD.ADMIN_ROLE}")]
        public async Task<IActionResult> Update(Cinema cinema, IFormFile ImgFile)
        {
            //var cinemaInDb = _context.Cinemas.AsNoTracking().FirstOrDefault(b => b.Id == cinema.Id);
            var cinemaInDb =await _cinemaRepository.GetOneAsync(filter: b => b.Id == cinema.Id,tracked:false);
            if (ImgFile != null && ImgFile.Length > 0)
            {
                //var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgFile.FileName);
                var fileName = Guid.NewGuid().ToString() + "-" + ImgFile.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\CinemaImages", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    ImgFile.CopyTo(stream);
                }
                cinema.Img = fileName;
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\CinemaImages", cinemaInDb.Img);

                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }
            else
            {
                cinema.Img = cinemaInDb.Img;
            }
            //_context.Cinemas.Update(cinema);
            //_context.SaveChanges();
            _cinemaRepository.Update(cinema);
            await _cinemaRepository.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = $"{CD.SUPER_ADMIN_ROLE},{CD.ADMIN_ROLE}")]
        public async Task<IActionResult> Delete(int id)
        {
            //var cinema = _context.Cinemas.FirstOrDefault(b => b.Id == id);
            var cinema =await _cinemaRepository.GetOneAsync(filter: b => b.Id == id);
            if (cinema == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\CinemaImages", cinema.Img);

            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }
            //_context.Cinemas.Remove(cinema);
            //_context.SaveChanges();
            _cinemaRepository.Delete(cinema);
            await _cinemaRepository.CommitAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
