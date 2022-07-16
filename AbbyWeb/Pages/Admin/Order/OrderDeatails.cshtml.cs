using Abby.DataAccess.Repository.IRepository;
using Abby.Model;
using Abby.Model.ViewModel;
using Abby.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;

namespace AbbyWeb.Pages.Admin.Order
{
    public class OrderDeatailsModel : PageModel
    {

        private readonly IUnitOfWork _unitOfWork;

        public OrderDeatailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public OrderDetailVM orderDetailVM { get; set; }
        public void OnGet(int id)
        {
            orderDetailVM = new OrderDetailVM()
            {
                orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id, includeProperties: "ApplicationUser"),
                orderDetails = _unitOfWork.OrderDetails.GetAll(u => u.OrderId == id).ToList()

            };
        }

        public IActionResult OnPostOrderComplete(int orderId)
        {
            _unitOfWork.OrderHeader.UpdateOrderStatus(orderId, SD.StatusCompleted);
            _unitOfWork.Save();
            return RedirectToPage("OrderList");
        }

        public IActionResult OnPostOrderRefund(int orderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId);
            var options = new RefundCreateOptions
            {
                Reason = RefundReasons.RequestedByCustomer,
                PaymentIntent = orderHeader.PaymentIntentId
            };
            var service = new RefundService();
            Refund refund = service.Create(options);
            _unitOfWork.OrderHeader.UpdateOrderStatus(orderId, SD.StatusRefunded);
            _unitOfWork.Save();
            return RedirectToPage("OrderList");
        }

        public IActionResult OnPostOrderCancel(int orderId)
        {
            _unitOfWork.OrderHeader.UpdateOrderStatus(orderId, SD.StatusCancelled);
            _unitOfWork.Save();
            return RedirectToPage("OrderList");
        }




    }
}
