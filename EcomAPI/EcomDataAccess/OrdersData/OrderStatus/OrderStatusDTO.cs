using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.OrdersData.OrderStatus
{
    public class OrderStatusDTO
    {
        public int OrderStatusID { set; get; }
        public string StatusName { set; get; }
        public OrderStatusDTO(int OrderStatusID,string StatusName)
        {
            this.OrderStatusID = OrderStatusID;
            this.StatusName = StatusName;
        }
    }
}
