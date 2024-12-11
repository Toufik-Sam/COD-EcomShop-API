namespace EcomDataAccess.SalesData.SalesManagement
{
    public class DailySalesSummaryDTO
    {
        public DateTime Date { set; get; }
        public decimal TotalRevenue { set; get; }
        public int TotalQuantitySold { set; get; }
        public int NumberOfShippedOrders { set; get; }
        public IList<SaleProductDTO> ProductsList { set; get; }
        public DailySalesSummaryDTO(DateTime Date, decimal TotalRevenue, int TotalQuantitySold,
            int NumberOfShippedOrders, IList<SaleProductDTO> ProductsList)
        {
            this.Date = Date;
            this.TotalRevenue = TotalRevenue;
            this.TotalQuantitySold = TotalQuantitySold;
            this.NumberOfShippedOrders = NumberOfShippedOrders;
            this.ProductsList = ProductsList;
        }
    }
}
