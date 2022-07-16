using Abby.DataAccess.Repository.IRepository;
using Abby.Model;
using Abby.Model.ViewModel;
using Abby.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.Order
{
    [Authorize(Roles = $"{SD.ManagerRole},{SD.KitchenRole}")]
    public class ManageOrderModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public List<OrderDetailVM> OrderDetails { get; set; }

        public ManageOrderModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;


        }


        public void OnGet()
        {
            OrderDetails = new();
            List<OrderHeader> orderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.Status == SD.StatusSubmitted ||
             u.Status == SD.StatusInProcess).ToList();

            foreach (OrderHeader item in orderHeaders)
            {
                OrderDetailVM individual = new OrderDetailVM()
                {
                    orderHeader = item,
                    orderDetails = _unitOfWork.OrderDetails.GetAll(u => u.OrderId == item.Id).ToList()
                };
                OrderDetails.Add(individual);
            }
        }

        public IActionResult OnPostOrderInProcess(int orderId)
        {
            _unitOfWork.OrderHeader.UpdateOrderStatus(orderId, SD.StatusInProcess);
            _unitOfWork.Save();
            return RedirectToPage("ManageOrder");
        }

        public IActionResult OnPostOrderReady(int orderId)
        {
            _unitOfWork.OrderHeader.UpdateOrderStatus(orderId, SD.StatusReady);
            _unitOfWork.Save();
            return RedirectToPage("ManageOrder");
        }

        public IActionResult OnPostOrderCancle(int orderId)
        {
            _unitOfWork.OrderHeader.UpdateOrderStatus(orderId, SD.StatusCancelled);
            _unitOfWork.Save();
            return RedirectToPage("ManageOrder");
        }

    }
}
