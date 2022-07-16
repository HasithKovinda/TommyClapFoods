using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abby.Model.ViewModel
{
    public class OrderDetailVM
    {
        public OrderHeader orderHeader { get; set; }

        public List<OrderDetails> orderDetails { get; set; }
    }
}
