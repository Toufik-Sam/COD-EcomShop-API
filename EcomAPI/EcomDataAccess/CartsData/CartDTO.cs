using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.CartsData
{
    public class CartDTO
    {
        public int CartID { set; get; }
        public int CustomerID { set; get; }
        public CartDTO(int CartID, int CustomerID)
        {
            this.CartID = CartID;
            this.CustomerID = CustomerID;
        }
    }
}
