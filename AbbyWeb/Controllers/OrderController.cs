using Abby.DataAccess.Repository.IRepository;
using Abby.Utility;
using Microsoft.AspNetCore.Mvc;

namespace AbbyWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        [HttpGet]
        public IActionResult Get(string? status = null)
        {
            var orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");

            if (status == "cancelled")
            {
                orderHeaders = orderHeaders.Where(u => u.Status == SD.StatusCancelled || u.Status == SD.StatusRejected);
            }
            else
            {
                if (status == "completed")
                {
                    orderHeaders = orderHeaders.Where(u => u.Status == SD.StatusCompleted);
                }
                else
                {
                    if (status == "ready")
                    {
                        orderHeaders = orderHeaders.Where(u => u.Status == SD.StatusReady);
                    }
                    else
                    {
                        orderHeaders = orderHeaders.Where(u => u.Status == SD.StatusSubmitted || u.Status == SD.StatusInProcess);
                    }
                }
            }

            return Json(new { data = orderHeaders });
        }
    }
}
