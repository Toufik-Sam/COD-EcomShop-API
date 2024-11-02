using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.OrdersData.OrderItems
{
    public class OrderItemDTO
    {
        public int OrderItemID { set; get; }
        public int ProductID { set; get; }
        public int OrderID { set; get; }
        public decimal Price { set; get; }
        public int Quantity { set; get; }
        public OrderItemDTO(int OrderItemID,int ProductID,int OrderID,decimal Price,int Quantity)
        {
            this.OrderItemID = OrderItemID;
            this.ProductID = ProductID;
            this.OrderID = OrderID;
            this.Price = Price;
            this.Quantity = Quantity;
        }
    }
}
