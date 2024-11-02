using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.CartsData.CartsItemsData
{
    public class CartItemDTO
    {
        public int CartItemID { set; get; }
        public int CartID { set; get; }
        public int ProductID { set; get; }
        public int Quantity { set; get; }
        public CartItemDTO(int CartItemID, int CartID, int ProductID, int Quantity)
        {
            this.CartItemID = CartItemID;
            this.CartID = CartID;
            this.ProductID = ProductID;
            this.Quantity = Quantity;
        }
    }
}
