using BookBazaar.Entities;
using BookBazaar.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BookBazaar.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork<Book> _Book;
        private readonly IUnitOfWork<ShoppingCart> _shoppingCart;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IUnitOfWork<Book> Book,
            IUnitOfWork<ShoppingCart> shoppingCart,
            ILogger<HomeController> logger)
        {
            _Book = Book;
            _shoppingCart = shoppingCart;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var Book = await _Book.Entity.GetAllAsync(filter: null, c=>c.Category, a=>a.Author, r=>r.Cover);
            return View(Book);
        }

        // GET: Home/Details/5
        public async Task<IActionResult> Details(int BookId)
        {

            ShoppingCart cart = new()
            {
                Book = await _Book.Entity.GetFirstOrDefaultAsync(p => p.Id == BookId, includeProperties: "Category,Author,Cover"),
                BookId = BookId,
                Count = 0
            };

            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Details(ShoppingCart shoppingCart)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = userId;

            var shoppingCartInDb = await _shoppingCart.Entity.GetFirstOrDefaultAsync(c => c.ApplicationUserId == userId && c.BookId == shoppingCart.BookId);
            var Book = await _Book.Entity.GetFirstOrDefaultAsync(p => p.Id == shoppingCart.BookId);


            if (shoppingCart.Count > Book.InStock)
            {
                TempData["Error"] = "Not enough items In stock.";
                return RedirectToAction(nameof(Index));
            }

            if (shoppingCartInDb == null)
            {
                await _shoppingCart.Entity.InsertAsync(shoppingCart);
                await _shoppingCart.CompleteAsync();

                Book.InStock -= shoppingCart.Count;
                await _Book.Entity.UpdateAsync(Book);
                await _Book.CompleteAsync();
            }

            else
            {
                await _shoppingCart.Entity.UpdateAsync(shoppingCartInDb);

                shoppingCartInDb.Count += shoppingCart.Count;

                if (Book.InStock > 0 && shoppingCart.Count <= Book.InStock)
                {
                    Book.InStock -= shoppingCart.Count;

                    await _Book.Entity.UpdateAsync(Book);
                    await _Book.CompleteAsync();

                    await _shoppingCart.CompleteAsync();
                }
                else
                {
                    TempData["Error"] = "Not enough items In stock.";
                    return RedirectToAction(nameof(Index));
                }
                
            }
 
            return RedirectToAction(actionName:"Index",controllerName:"Cart");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}