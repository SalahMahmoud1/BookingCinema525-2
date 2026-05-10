using BookingCinema525_new.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookingCinema525_new.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user =await _userManager.GetUserAsync(User);
            if(user is null)
            {
                return View();
            }
            //var applictionUserVM = new ApplicationUserVM();
            //applictionUserVM.Name = user.Name;
            //applictionUserVM.Email = user.Email;
            //applictionUserVM.PhoneNumber = user.PhoneNumber;
            //applictionUserVM.Address = user.Address;
            var applictionUserVM = user.Adapt<ApplicationUserVM>();
            return View(applictionUserVM);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ApplicationUserVM applicationUserVM)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return View();
            }
            user.Name = applicationUserVM.Name;
            user.PhoneNumber = applicationUserVM.PhoneNumber;
            user.Address = applicationUserVM.Address;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                TempData["Error-Notification"] = "Invalid data";
                return RedirectToAction(nameof(Index),applicationUserVM);
            }
            TempData["Success-Notification"] = "updated user Successfully";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(ApplicationUserVM applicationUserVM)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return View();
            }
            var result = await _userManager.ChangePasswordAsync(user, applicationUserVM.CurrentPassword, applicationUserVM.NewPassword);
            if (!result.Succeeded)
            {
                var erros = string.Join(", ", result.Errors.Select(e => e.Description));
                TempData["Error-Notification"] = erros;
                return View(nameof(Index), applicationUserVM);
            }
            TempData["Success-Notification"] = "change password Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
