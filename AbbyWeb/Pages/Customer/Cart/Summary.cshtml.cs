using Abby.DataAccess.Repository.IRepository;
using Abby.Model;
using Abby.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;
using System.Security.Claims;

namespace AbbyWeb.Pages.Customer.Cart
{
    [Authorize]
    [BindProperties]
    public class SummaryModel : PageModel
    {
        public IEnumerable<ShoppingCart> shoppingCarts { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        public OrderHeader OrderHeader { get; set; }
        public SummaryModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            OrderHeader = new OrderHeader();

        }
        public void OnGet()
        {
            var clamIdentity = User.Identity as ClaimsIdentity;
            var clam = clamIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (clam != null)
            {
                shoppingCarts = _unitOfWork.ShoppingCart.GetAll(filter: u => u.ApplicationUserId == clam.Value,
                    includeProperties: "MenuItem,MenuItem.FoodType,MenuItem.Category");

                foreach (var shoppingCart in shoppingCarts)
                {
                    OrderHeader.OrderTotal += (shoppingCart.Count * shoppingCart.MenuItem.Price);
                }

                ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == clam.Value);
                OrderHeader.PickupName = applicationUser.FirstName + " " + applicationUser.LastName;
                OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
            }
        }

        public IActionResult OnPost()

        {
            var clamIdentity = User.Identity as ClaimsIdentity;
            var clam = clamIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (clam != null)
            {
                shoppingCarts = _unitOfWork.ShoppingCart.GetAll(filter: u => u.ApplicationUserId == clam.Value,
                    includeProperties: "MenuItem,MenuItem.FoodType,MenuItem.Category");

                foreach (var shoppingCart in shoppingCarts)
                {
                    OrderHeader.OrderTotal += (shoppingCart.Count * shoppingCart.MenuItem.Price);
                }

                OrderHeader.Status = SD.StatusPending;
                OrderHeader.OrderDate = DateTime.Now;
                OrderHeader.UserId = clam.Value;
                OrderHeader.PickUpTime = Convert.ToDateTime(OrderHeader.PickUpDate.ToShortDateString() + " " + OrderHeader.PickUpTime.ToShortTimeString());
                _unitOfWork.OrderHeader.Add(OrderHeader);
                _unitOfWork.Save();

                foreach (var item in shoppingCarts)
                {

                    OrderDetails orderDetails = new OrderDetails()
                    {
                        MenuItemId = item.MenuItemId,
                        OrderId = OrderHeader.Id,
                        Name = item.MenuItem.Name,
                        Price = item.MenuItem.Price,
                        Count = item.Count
                    };
                    _unitOfWork.OrderDetails.Add(orderDetails);


                }
                // _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);

                _unitOfWork.Save();

                var domain = "https://localhost:7123/";
                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>(),
                    PaymentMethodTypes = new List<string>
                    {
                        "card"
                    },
                    Mode = "payment",
                    SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={OrderHeader.Id}",
                    CancelUrl = domain + "Customer/Cart/Index",
                };

                foreach (var item in shoppingCarts)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)item.MenuItem.Price * 100,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.MenuItem.Name,
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }
                var service = new SessionService();
                Session session = service.Create(options);
                Response.Headers.Add("Location", session.Url);
                OrderHeader.SessionId = session.Id;
                OrderHeader.PaymentIntentId = session.PaymentIntentId;
                _unitOfWork.Save();
                return new StatusCodeResult(303);


            }

            return Page();
        }
    }
}


