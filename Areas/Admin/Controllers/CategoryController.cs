using BookingCinema525.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookingCinema525.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //ApplicationDbContext _context = new ApplicationDbContext();
        IRepository<Category> _categoryRepository; //= new Repository<Category>();

        public CategoryController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            //var categories = _context.Categories.AsQueryable();
            var categories =await _categoryRepository.GetAsync();
            return View(categories.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            //_context.Categories.Add(category);
            await _categoryRepository.CreateAsync(category);
            //_context.SaveChanges();
            await _categoryRepository.CommitAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            //var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            var category = await _categoryRepository.GetOneAsync(filter: c => c.Id == id);
            if (category == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            return View(category);
           
        }
        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
            //_context.Categories.Update(category);
            _categoryRepository.Update(category);
            //_context.SaveChanges();
            await _categoryRepository.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            //var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            var category =await _categoryRepository.GetOneAsync(filter: c => c.Id == id);
            if (category == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            //_context.Categories.Remove(category);
            _categoryRepository.Delete(category);
            //_context.SaveChanges();
            await _categoryRepository.CommitAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
