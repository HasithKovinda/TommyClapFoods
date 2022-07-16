using Abby.DataAccess.Repository.IRepository;
using Abby.Model;
using Abby.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace AbbyWeb.Pages.Customer.Cart
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public IEnumerable<ShoppingCart> shoppingCarts { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        public double Total;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Total = 0;
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
                    Total += (shoppingCart.Count * shoppingCart.MenuItem.Price);
                }
            }
        }

        public IActionResult OnPostPlus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.IncrementShoppingCart(cart, 1);
            return RedirectToPage("/Customer/Cart/Index");
        }

        public IActionResult OnPostMinus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            if (cart.Count == 1)
            {
                var count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count() - 1;
                _unitOfWork.ShoppingCart.Remove(cart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart, count);
            }
            else
            {
                _unitOfWork.ShoppingCart.DecrementShoppingCart(cart, 1);

            }

            return RedirectToPage("/Customer/Cart/Index");
        }

        public IActionResult OnPostRemove(int cartId)
        {

            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            var count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count() - 1;

            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.SessionCart, count);
            return RedirectToPage("/Customer/Cart/Index");
        }
    }
}
