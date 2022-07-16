using Abby.DataAccess.Repository.IRepository;
using Abby.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AbbyWeb.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var clamIdentity = User.Identity as ClaimsIdentity;
            var clam = clamIdentity.FindFirst(ClaimTypes.NameIdentifier);
            int count = 0;

            if (clam != null)
            {
                //check session is set and retrive the values
                if (HttpContext.Session.GetInt32(SD.SessionCart) != null)
                {
                    //session has value
                    return View(HttpContext.Session.GetInt32(SD.SessionCart));
                }
                else
                {
                    //no session set
                    count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == clam.Value).ToList().Count;
                    HttpContext.Session.SetInt32(SD.SessionCart, count);
                    return View(count);

                }
            }
            else
            {
                HttpContext.Session.Clear();
                return View(count);
            }
        }

    }
}
