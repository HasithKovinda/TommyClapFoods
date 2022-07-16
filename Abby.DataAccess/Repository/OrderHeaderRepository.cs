using Abby.DataAccess.Repository.IRepository;
using Abby.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Abby.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader obj)
        {
            _db.OrderHeader.Update(obj);

        }

        public void UpdateOrderStatus(int id, string status)
        {
            var resFormDb = _db.OrderHeader.FirstOrDefault(u => u.Id == id);
            if (resFormDb != null)
            {
                resFormDb.Status = status;
            }

        }
    }
}
