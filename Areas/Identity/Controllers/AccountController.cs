using BookingCinema525.Repositories;
using BookingCinema525_new.Models;
using BookingCinema525_new.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace BookingCinema525_new.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        IEmailSender _emailSender;
        IRepository<ApplicationUserOTP> _applicationUserOTP;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, IRepository<ApplicationUserOTP> applicationUserOTP)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _applicationUserOTP = applicationUserOTP;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if(!ModelState.IsValid)
            {
                return View(registerVM);
            }
            ApplicationUser user = new ApplicationUser ();
            user.Name =registerVM .Name;
            user.UserName =registerVM .UserName;
            user.Email =registerVM .Email;
            user.Address = registerVM.Address;
            user.PasswordHash = registerVM.Password;
            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                string errors = "";
                foreach (var error in result.Errors)
                {
                    errors += error.Description + "\n";
                }
                ModelState.AddModelError(string.Empty, errors);
                return View(registerVM);
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { area = "Identity", userId = user.Id , token }, Request.Scheme);
            await _emailSender.SendEmailAsync(
                registerVM.Email,
                "Confirm Your Email from Booking Cinema525",
                $"<h1> click <a href= {link} > here </a> to confirm your mail </h1>"
                );
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> ConfirmEmail(string UserId, string token)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user is null)
            {
                TempData["Error-Notification"] = "Invalid User";
                return RedirectToAction(nameof(Login));
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                TempData["Error-Notification"] = result.Errors;
                return RedirectToAction(nameof(Login));
            }
            TempData["Success-Notification"] = "Email Confirmed successfully";
            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public IActionResult ResendEmailConfirmation()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationVM resendEmailConfirmationVM)
        {
            var user = await _userManager.FindByEmailAsync(resendEmailConfirmationVM.UserNameOrEmail) ??
                       await _userManager.FindByNameAsync(resendEmailConfirmationVM.UserNameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError("", "invalid UserName Or Email Or Password");
                return View(resendEmailConfirmationVM);
            }
            if(user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Your Email is already confirmed");
                return View(resendEmailConfirmationVM);
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { area = "Identity", userId = user.Id, token }, Request.Scheme);
            await _emailSender.SendEmailAsync(
                user.Email,
                "Confirm Your Email from Booking Cinema525",
                $"<h1> click <a href= {link} > here </a> to confirm your mail </h1>"
                );
            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var user =await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail) ?? await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
            if(user == null)
            {
                ModelState.AddModelError("", "Invalid User Name or Email OR Password");
                return View(loginVM);
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);
            if (!result.Succeeded)
            {
                if(result.IsLockedOut)
                {
                    ModelState.AddModelError("", "to many attempets try agin later");
                }
                return View(loginVM);
            }
            TempData["Success-Notification"] = "Login Successfully";
            return RedirectToAction("Index", "HomeMovie", new { area = "Main" });
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordVM.UserNameOrEmail) ??
                       await _userManager.FindByNameAsync(forgetPasswordVM.UserNameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError("", "invalid UserName Or Email Or Password");
                return View(forgetPasswordVM);
            }
            var otp = new Random().Next(1000, 9999).ToString();
            var applicationUserOtp = new ApplicationUserOTP(otp, user.Id);
            await _applicationUserOTP.CreateAsync(applicationUserOtp);
            await _applicationUserOTP.CommitAsync();
            await _emailSender.SendEmailAsync(
                user.Email,
                "Rest Your Password",
                $"<h1> Use This <span style = \"color:red\" > {otp} </span> to rest your password </h1>"
                );

            return RedirectToAction(nameof(VerifyOTP), new { userId = user.Id });
        }
        [HttpGet]
        public IActionResult VerifyOTP(string userId)
        {
            return View(new VerifyOTPVM() { UserId = userId });
        }
        [HttpPost]
        public async Task<IActionResult> VerifyOTP(VerifyOTPVM verifyOTPVM)
        {
            var user = await _userManager.FindByIdAsync(verifyOTPVM.UserId);
            if (user == null)
            {
                ModelState.AddModelError("", "invalid UserName Or Email Or Password");
                return View(verifyOTPVM);
            }

            var otps = await _applicationUserOTP.GetAsync(e =>
                e.ApplicationUserId == user.Id &&
                e.IsValid == true &&
                DateTime.UtcNow < e.ValidTo &&
                e.OTP == verifyOTPVM.OTP
                );
            var otp =otps.OrderByDescending (e => e.CreatedAt).FirstOrDefault();
            if (otp == null || otp.OTP!=verifyOTPVM.OTP)
            {
                ModelState.AddModelError("", "Invalid / Expired OTP ");
                return View(verifyOTPVM);
            }
            otp.IsValid = false;
            await _applicationUserOTP.CommitAsync();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            TempData["token"] = token;
            return RedirectToAction(nameof(ResetPassword), new { userId = user.Id });
        }
        [HttpGet]
        public IActionResult ResetPassword(string userId)
        {
            var token = TempData["token"] as string;
            if (token is null)
            {
                return RedirectToAction(nameof(Login));
            }
            return View(new ResetPasswordVM() { UserId = userId, Token = token });
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {

            if (resetPasswordVM.Token is null)
            {
                return RedirectToAction(nameof(Login));
            }
            var user = await _userManager.FindByIdAsync(resetPasswordVM.UserId);
            if (user == null)
            {
                ModelState.AddModelError("", "invalid User ");
                return View(resetPasswordVM);
            }
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Token, resetPasswordVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(resetPasswordVM);
            }
            return RedirectToAction(nameof(Login));
        }

    }
}
