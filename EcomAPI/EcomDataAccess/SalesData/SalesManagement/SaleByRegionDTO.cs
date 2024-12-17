namespace EcomDataAccess.SalesData.SalesManagement
{
    public class SaleByRegionDTO
    {
        public string Country { set; get; }
        public string City { set; get; }
        public decimal TotalSales { set; get; }
        public int TotalQuantitySold { set; get; }
        public int NumberOfShippedOrders { set; get; }
        public DateTime FirstDatePurchase { set; get; }
        public DateTime LastDatePurchase { set; get; }
        public IList<SaleProductDTO>ProductsList { set; get; }
        public SaleByRegionDTO(string Country,string City,decimal TotalSales,int TotalQuantitySold,int NumberOfShippedOrders,
            DateTime FirstDatePurchase,DateTime LastDatePurchase,IList<SaleProductDTO>ProductList)
        {
            this.Country = Country;
            this.City = City;
            this.TotalSales = TotalSales;
            this.TotalQuantitySold = TotalQuantitySold;
            this.NumberOfShippedOrders = NumberOfShippedOrders;
            this.FirstDatePurchase = FirstDatePurchase;
            this.LastDatePurchase = LastDatePurchase;
            this.ProductsList = ProductsList;
        }

    }
}
