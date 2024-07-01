using BookBazaar.Entities;
using BookBazaar.ViewModels;
using BookBazaar.Interfaces;
using BookBazaar.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
//using Stripe.Checkout;
using System.Security.Claims;

namespace BookBazaar.Areas.Customer.Controllers
{
#pragma warning disable

    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork<Book> _Book;
        private readonly IUnitOfWork<ShoppingCart> _shoppingCart;
        private readonly IUnitOfWork<ApplicationUser> _applicationUser;
        private readonly IUnitOfWork<OrderHeader> _orderHeader;
        private readonly IUnitOfWork<OrderDetail> _orderDetail;
        private readonly IEmailSender _emailSender;


        private IConfiguration _configuration { get; }

        [BindProperty]
        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }

        public CartController(
            IUnitOfWork<Book> Book,
            IUnitOfWork<ShoppingCart> shoppingCart,
            IUnitOfWork<ApplicationUser> applicationUser,
            IUnitOfWork<OrderHeader> orderHeader,
            IUnitOfWork<OrderDetail> orderDetail,
            IConfiguration configuration,
            IEmailSender emailSender)
        {
            _Book = Book;
            _shoppingCart = shoppingCart;
            _applicationUser = applicationUser;
            _orderHeader = orderHeader;
            _orderDetail = orderDetail;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        // GET: CartController
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ShoppingCartViewModel = new ShoppingCartViewModel()
            {
                ShoppingCarts = await _shoppingCart.Entity.GetAllAsync(c => c.ApplicationUserId == userId, includeProperties: p => p.Book),
                OrderHeader = new()
            };

            foreach (var item in ShoppingCartViewModel.ShoppingCarts)
            {
                item.PriceHolder = item.Count * item.Book.Price;
                item.SetPriceHolderRabat(item.Count * item.Book.Price * 0.9);
                ShoppingCartViewModel.OrderHeader.OrderTotal += (item.PriceHolderRabat());
            }

            return View(ShoppingCartViewModel);
        }

        /// <summary>
        /// Add new item to the shopping basket.
        /// </summary>
        /// <param name="cartId">Shopping cart Id</param>
        /// <returns></returns>
        public async Task<IActionResult> AddItem(Guid? cartId)
        {
            ShoppingCart? cart = await _shoppingCart.Entity.GetFirstOrDefaultAsync(u => u.Id == cartId, includeProperties: "Book");

            if (cart != null)
            {
                if (cart.Book.InStock > 0)
                {
                    cart.Count += 1;
                    await _shoppingCart.Entity.UpdateAsync(cart);
                    await _shoppingCart.CompleteAsync();

                    cart.Book.InStock -= 1;
                    await _Book.Entity.UpdateAsync(cart.Book);
                    await _Book.CompleteAsync();
                }
                else
                {
                    TempData["Error"] = "Not enough items In stock.";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Remove items from the shopping basket.
        /// </summary>
        /// <param name="cartId">Shopping cart Id</param>
        /// <returns></returns>
        public async Task<IActionResult> RemoveItem(Guid? cartId)
        {
            ShoppingCart? cart = await _shoppingCart.Entity.GetFirstOrDefaultAsync(u => u.Id == cartId, includeProperties: "Book");

            if (cart != null)
            {
                if (cart.Count > 0)
                {
                    cart.Count -= 1;
                    await _shoppingCart.Entity.UpdateAsync(cart);
                    await _shoppingCart.CompleteAsync();

                    cart.Book.InStock += 1;
                    await _Book.Entity.UpdateAsync(cart.Book);
                    await _Book.CompleteAsync();
                }

            }

            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// Remove item from the shopping cart.
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public async Task<IActionResult> RemoveFromBasket(Guid cartId)
        {
            ShoppingCart? cart = await _shoppingCart.Entity.GetFirstOrDefaultAsync(u => u.Id == cartId, includeProperties: "Book");
            var items = cart.Count;

            if (cart != null)
            {
                cart.Count -= items;
                await _shoppingCart.Entity.UpdateAsync(cart);
                await _shoppingCart.CompleteAsync();

                cart.Book.InStock += items;
                await _Book.Entity.UpdateAsync(cart.Book);
                await _Book.CompleteAsync();
            }

            await _shoppingCart.Entity.DeleteAsync(cartId);
            await _shoppingCart.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// Show Order summary.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Summary()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ShoppingCartViewModel = new ShoppingCartViewModel()
            {
                ShoppingCarts = await _shoppingCart.Entity.GetAllAsync(
                c => c.ApplicationUserId == userId,
                includeProperties: p => p.Book),
                OrderHeader = new()
            };

            ShoppingCartViewModel.OrderHeader.ApplicationUser = await _applicationUser.Entity.GetFirstOrDefaultAsync(u => u.Id == userId);
            ShoppingCartViewModel.OrderHeader.FirstName = ShoppingCartViewModel.OrderHeader.ApplicationUser.FirstName;
            ShoppingCartViewModel.OrderHeader.LastName = ShoppingCartViewModel.OrderHeader.ApplicationUser.LastName;
            ShoppingCartViewModel.OrderHeader.PhoneNumber = ShoppingCartViewModel.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartViewModel.OrderHeader.StreetAddress = ShoppingCartViewModel.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartViewModel.OrderHeader.City = ShoppingCartViewModel.OrderHeader.ApplicationUser.City;
            ShoppingCartViewModel.OrderHeader.State = ShoppingCartViewModel.OrderHeader.ApplicationUser.State;
            ShoppingCartViewModel.OrderHeader.PostalCode = ShoppingCartViewModel.OrderHeader.ApplicationUser.PostalCode;

            foreach (var item in ShoppingCartViewModel.ShoppingCarts)
            {
                item.PriceHolder = item.Count * item.Book.Price;
                item.SetPriceHolderRabat(item.Count * item.Book.Price * 0.9);
                ShoppingCartViewModel.OrderHeader.OrderTotal += (item.PriceHolderRabat());
            }

            if (!(ShoppingCartViewModel.OrderHeader.OrderTotal > 0))
                return RedirectToAction(actionName: "Index", controllerName: "Cart");

            return View(ShoppingCartViewModel);
        }


        /// <summary>
        /// Show Order summary.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SetSummary()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appUser = await _applicationUser.Entity.GetFirstOrDefaultAsync(a => a.Id == userId, includeProperties: "Company");

            ShoppingCartViewModel.ShoppingCarts = await _shoppingCart.Entity.GetAllAsync(c => c.ApplicationUserId == userId, includeProperties: p => p.Book);

            // Shipping Details
            ShoppingCartViewModel.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartViewModel.OrderHeader.ApplicationUserId = userId;
            ShoppingCartViewModel.OrderHeader.Company = appUser.Company;

            if (appUser.CompanyId.GetValueOrDefault() == 0)
            {
                ShoppingCartViewModel.OrderHeader.PaymentStatus = Status.Payment.Pending.ToString();
                ShoppingCartViewModel.OrderHeader.OrderStatus = Status.StatusType.Pending.ToString();
            }
            else
            {
                ShoppingCartViewModel.OrderHeader.PaymentStatus = Status.Payment.Delayed.ToString();
                ShoppingCartViewModel.OrderHeader.OrderStatus = Status.StatusType.Approved.ToString();
            }


            foreach (var item in ShoppingCartViewModel.ShoppingCarts)
            {
                item.PriceHolder = item.Count * item.Book.Price;
                item.SetPriceHolderRabat(item.Count * item.Book.Price * 0.9);
                ShoppingCartViewModel.OrderHeader.OrderTotal += (item.PriceHolderRabat());
            }

            await _orderHeader.Entity.InsertAsync(ShoppingCartViewModel.OrderHeader);
            await _orderHeader.CompleteAsync();


            // Order Summary
            foreach (var item in ShoppingCartViewModel.ShoppingCarts)
            {
                OrderDetail oderDetail = new()
                {
                    BookId = item.BookId,
                    OrderId = ShoppingCartViewModel.OrderHeader.Id,
                    Price = item.PriceHolderRabat(),
                    Count = item.Count
                };

                await _orderDetail.Entity.InsertAsync(oderDetail);
                await _orderDetail.CompleteAsync();
            }

            // Check if individual or company user
            if (appUser.CompanyId.GetValueOrDefault() != 0)
                return RedirectToAction("OrderConfirmed", "Cart", new { id = ShoppingCartViewModel.OrderHeader.Id });

            ////stripe settings 
            //var domain = _configuration["StripeSettings:Url"];
            //var options = new SessionCreateOptions
            //{
            //    PaymentMethodTypes = new List<string>
            //    {
            //        "card",
            //    },
            //    LineItems = new List<SessionLineItemOptions>(),
            //    Mode = "payment",
            //    SuccessUrl = domain + $"customer/cart/OrderConfirmed?id={ShoppingCartViewModel.OrderHeader.Id}",
            //    CancelUrl = domain + $"customer/cart/index",
            //};

            //foreach (var item in ShoppingCartViewModel.ShoppingCarts)
            //{

            //    var sessionLineItem = new SessionLineItemOptions
            //    {
            //        PriceData = new SessionLineItemPriceDataOptions
            //        {
            //            UnitAmount = (long)(item.SetPriceHolderRabat(item.Book.Price * 0.9) * 100), //20.00 -> 2000
            //            Currency = "sek",
            //            BookData = new SessionLineItemPriceDataBookDataOptions
            //            {
            //                Name = item.Book.Title,
            //            },
            //        },
            //        Quantity = item.Count
            //    };
            //    options.LineItems.Add(sessionLineItem);
            //}

            //var service = new SessionService();
            //Stripe.Checkout.Session session = service.Create(options);
            //session.ExpiresAt = DateTime.Now.AddSeconds(5);

            ////Update orderheader table.
            //ShoppingCartViewModel.OrderHeader.SessionId = session.Id;
            //ShoppingCartViewModel.OrderHeader.PaymentIntentId = session.PaymentIntentId;
            //await _orderHeader.Entity.UpdateAsync(ShoppingCartViewModel.OrderHeader);
            //await _orderHeader.CompleteAsync();


            //Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public async Task<IActionResult> OrderConfirmed(int id)
        {
            var orderHeaderInDb = await _orderHeader.Entity.GetFirstOrDefaultAsync(o => o.Id == id, includeProperties: "ApplicationUser");

            if (!orderHeaderInDb.PaymentStatus.Equals(Status.Payment.Delayed.ToString()))
            {
                //var service = new SessionService();
                //Stripe.Checkout.Session session = service.Get(orderHeaderInDb.SessionId);

                //// Check stripe status.
                //if (session.PaymentStatus.ToLower().Equals("paid"))
                //{
                //    orderHeaderInDb.OrderStatus = Status.StatusType.Approved.ToString();
                //    orderHeaderInDb.PaymentStatus = Status.Payment.Approved.ToString();
                //    orderHeaderInDb.PaymentDate = DateTime.Now;

                //    await _orderHeader.Entity.UpdateAsync(orderHeaderInDb);
                //    await _orderHeader.CompleteAsync();

                //    await _emailSender.SendEmailAsync(orderHeaderInDb.ApplicationUser.Email,
                //        "New Order - BookBazaar webapp",
                //       $"<p>New order {orderHeaderInDb.Id} has been created.<p/>");

                //    // Decrease Book in stock
                //    ShoppingCart? cart = await _shoppingCart.Entity.GetFirstOrDefaultAsync(u => u.ApplicationUserId == orderHeaderInDb.ApplicationUserId, includeProperties: "Book");
                //    OrderDetail detail = await _orderDetail.Entity.GetFirstOrDefaultAsync(u => u.OrderId == orderHeaderInDb.Id, includeProperties: "Book");

                //    cart.Book.InStock -= detail.Count;
                //    await _shoppingCart.Entity.UpdateAsync(cart);
                //    await _shoppingCart.CompleteAsync();
               // }
            }

            // Retreive shoppingcarts from the database.
            var shoppingCartsInDb = await _shoppingCart.Entity.GetAllAsync(u => u.ApplicationUserId == orderHeaderInDb.ApplicationUserId, includeProperties: p => p.Book);

            // Remove shopping carts from database.
            await _shoppingCart.Entity.DeleteRangeAsync(shoppingCartsInDb);
            await _shoppingCart.CompleteAsync();

            // Send sms confirmation
            //TwilioClient.Init(_twilio.AccountSid, _twilio.AuthToken);
            //try
            //{
            //    var message = MessageResource.Create(
            //        body: "Your order " + orderHeaderInDb.OrderTotal.ToString("c") + " with Id:" + id + " was placed Successfully.",
            //        from: new Twilio.Types.PhoneNumber(_twilio.PhoneNumber),
            //        to: new Twilio.Types.PhoneNumber(orderHeaderInDb.PhoneNumber)
            //        );
            //}
            //catch (Exception ex)
            //{

            //    _logger.LogError("Please verify the phone number.", ex);
            //}

            return View(id);
        }
    }
}
