using Abby.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abby.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository: IRepository<ShoppingCart>
    {
        public int IncrementShoppingCart(ShoppingCart shoppingCart,int value);
        public int DecrementShoppingCart(ShoppingCart shoppingCart, int value);
    }
}
