using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookingCinema525_new.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles =$"{CD.SUPER_ADMIN_ROLE},{CD.ADMIN_ROLE}")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.AsQueryable();
            return View(users.AsEnumerable());
        }
        public async Task<IActionResult> LockUnLock(string id)
        {
            var user =await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            bool IsSuperAdmin = await _userManager.IsInRoleAsync(user, CD.SUPER_ADMIN_ROLE);
            if (IsSuperAdmin)
            {
                TempData["Error-Notification"] = "Dont have Permission To Lock Supper Admin";
                return RedirectToAction(nameof(Index));
            }
            else 
            {
                //Unlock
                if (user.LockoutEnd == null || DateTime.UtcNow > user.LockoutEnd)
                {
                    //Lock
                    //user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
                    await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddMinutes(15));
                }
                //Lock
                else
                {
                    //Unlock
                    //user.LockoutEnd = null;
                    await _userManager.SetLockoutEndDateAsync(user, null);
                }
            }
           
                return RedirectToAction(nameof(Index));
        }
    }
}
