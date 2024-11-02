using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.OrdersData
{
    public class OrderDTO
    {
        public int OrderID { set; get; }
        public int CustomerID { set; get; }
        public int OrderStatusID { set; get; }
        public DateTime CreatedAt { set; get; }
        public OrderDTO(int OrderID,int CustomerID,int OrderStatusID,DateTime CreatedAt)
        {
            this.OrderID = OrderID;
            this.CustomerID = CustomerID;
            this.OrderStatusID = OrderStatusID;
            this.CreatedAt = CreatedAt;
        }
    }
}
