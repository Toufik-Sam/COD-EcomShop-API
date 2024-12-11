using EcomDataAccess.OrdersData.OrderItems;

namespace EcomDataAccess.SalesData
{
    public class FullSaleDTO
    {
        public int SaleID { set; get; }
        public int OrderID { set; get; }
        public IList<OrderItemInfoDTO> OrderItems { set; get; }
        public decimal Amount { set; get; }
        public FullSaleDTO(int SaleID, int OrderID,IList<OrderItemInfoDTO> OrderItemsInfo, decimal Amount)
        {
            this.SaleID = SaleID;
            this.OrderID = OrderID;
            this.OrderItems = OrderItemsInfo;
            this.Amount = Amount;
        }
    }
}
