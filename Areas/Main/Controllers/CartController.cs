using BookingCinema525.Repositories;
using BookingCinema525_new.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace BookingCinema525_new.Areas.Main.Controllers
{
    [Area("Main")]
    public class CartController : Controller
    {
        private readonly IRepository<Cart> _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Movie> _movieRepository;
        private readonly IRepository<Promotion> _promotionRepository;
        private readonly IRepository<UserPromotion> _userPromotionRepository;

        public CartController(IRepository<Cart> cartRepository, IRepository<Movie> movieRepository, UserManager<ApplicationUser> userManager, IRepository<Promotion> promotionRepository, IRepository<UserPromotion> userPromotionRepository)
        {
            _cartRepository = cartRepository;
            _movieRepository = movieRepository;
            _userManager = userManager;
            _promotionRepository = promotionRepository;
            _userPromotionRepository = userPromotionRepository;
        }

        public async Task<IActionResult> Index(string code)
        {
            var user =await _userManager.GetUserAsync(User);
            if(user == null) return NotFound();
            if (code != null)
            {
                var promotion = await _promotionRepository.GetOneAsync(p =>
                        p.Code == code &&
                        p.IsValid &&
                        DateTime.UtcNow < p.ValidTo &&
                        p.MaxUsage > 0
                    );
                if (promotion != null)
                {
                    var cart = await _cartRepository.GetOneAsync(c => c.MovieId == promotion.MovieId && c.ApplicationUserId == user.Id);
                    if (cart != null)
                    {
                        var oldPromotion =await _userPromotionRepository.GetOneAsync(e=>e.PromotionId == promotion.Id && e.ApplicationUserId == user.Id);
                        if (oldPromotion == null)
                        {
                            cart.Price -= cart.Price * promotion.Discount / 100;
                            promotion.MaxUsage--;
                            var userPromotion = new UserPromotion()
                            {
                                ApplicationUserId = user.Id,
                                PromotionId = promotion.Id
                            };
                            await _userPromotionRepository.CreateAsync(userPromotion);
                            await _cartRepository.CommitAsync();
                            TempData["Success-Notification"] = "promotion Applied Successfully";
                        }
                        else 
                        {
                            TempData["Error-Notification"] = "You have usrd this Promotion before ";
                        }
                        
                    }
                    else
                    {
                        TempData["Error-Notification"] = "there is no Pormotion to apply this Movie For this user ";
                    }
                }
                else
                {
                    TempData["Error-Notification"] = "invalid / Expired Code";
                }

            }
            var carts =await _cartRepository.GetAsync(c=>c.ApplicationUserId == user.Id, includes: [c=>c.Movie]);
            var CarttotalPrice = carts.Sum(c => c.Price * c.Count);
            ViewBag.CartTotalPrice = CarttotalPrice;
            return View(carts);
        }
        public async Task<IActionResult> AddToCart(int movieId, int count)
        {
            var user =await _userManager.GetUserAsync(User);
            if(user == null) return NotFound();
            var movie =await _movieRepository.GetOneAsync(m => m.Id == movieId);
            if (movie == null) return NotFound();
            var cartInDb =await _cartRepository.GetOneAsync (c=>c.MovieId == movieId && c.ApplicationUserId == user.Id);
            if(cartInDb != null)
            {
                cartInDb.Count += count;
                await _cartRepository.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            var cart = new Cart()
            {
                MovieId = movieId,
                Count = count,
                ApplicationUserId = user.Id,
                Price = movie.Price - (movie.Price * (movie.Discount / 100))
            }; 
            await _cartRepository.CreateAsync(cart);
            await _cartRepository.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DecrementCount(int movieId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            var cartInDb = await _cartRepository.GetOneAsync(c => c.MovieId == movieId && c.ApplicationUserId == user.Id);
            if (cartInDb == null) return NotFound();
            if(cartInDb.Count > 1)
            {
                cartInDb.Count--;
                await _cartRepository.CommitAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> IncrementCount(int movieId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return NotFound();
            var cartInDb = await _cartRepository.GetOneAsync(c => c.MovieId == movieId && c.ApplicationUserId == user.Id);
            if (cartInDb is null) return NotFound();
            var movie = await _movieRepository.GetOneAsync(p => p.Id == movieId);
            if (movie is null) return NotFound();
            if (cartInDb.Count < movie.Quantity)
            {
                cartInDb.Count++;
                await _cartRepository.CommitAsync();
            }
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> DeleteMovie(int movieId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return NotFound();
            var cartInDb = await _cartRepository.GetOneAsync(c => c.MovieId == movieId && c.ApplicationUserId == user.Id);
            if (cartInDb is null) return NotFound();
            _cartRepository.Delete(cartInDb);
            await _cartRepository.CommitAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Pay()
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
            };
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return NotFound();
            var carts = await _cartRepository.GetAsync(c => c.ApplicationUserId == user.Id, includes: [c => c.Movie]);
            if (carts is null) return NotFound();
            foreach (var item in carts)
            {
                var sessionLineItemOptions = new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Movie.Name,
                            Description = item.Movie.Description,
                        },
                        UnitAmount = (long)item.Price * 100,
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItemOptions);
            }
            var service = new SessionService();
            var session = service.Create(options);
            return Redirect(session.Url);
        }

    }
}
