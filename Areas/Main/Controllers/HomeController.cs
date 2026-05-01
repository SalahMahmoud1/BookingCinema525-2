using Microsoft.AspNetCore.Mvc;

namespace BookingCinema525.Areas.Main.Controllers
{
    [Area("Main")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
