using EcomDataAccess.OrdersData.OrderItems;
namespace EcomDataAccess.SalesData
{
    public class SaleDTO
    {
        public int SaleID { set; get; }
        public int OrderID { set; get; }
        public decimal Amount { set; get; }
        public SaleDTO(int SaleID,int OrderID,decimal Amount)
        {
            this.SaleID = SaleID;
            this.OrderID = OrderID;
            this.Amount = Amount;
        }
    }
}
