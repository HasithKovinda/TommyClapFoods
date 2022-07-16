using Abby.DataAccess.Repository.IRepository;
using Abby.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abby.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public int DecrementShoppingCart(ShoppingCart shoppingCart, int value)
        {
            shoppingCart.Count -= value;
            _db.SaveChanges();
            return shoppingCart.Count;
        }

        public int IncrementShoppingCart(ShoppingCart shoppingCart, int value)
        {
            shoppingCart.Count += value;
            _db.SaveChanges();
            return shoppingCart.Count;
        }
    }
}
