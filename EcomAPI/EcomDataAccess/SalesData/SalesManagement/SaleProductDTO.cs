namespace EcomDataAccess.SalesData.SalesManagement
{
    public class SaleProductDTO
    {
        public int ProductID { set; get; }
        public string ProductName { set; get; }
        public decimal TotalRevenue { set; get; }
        public decimal Price { set; get;}
        public int QuantitySold { set; get; }
        public SaleProductDTO(int ProductID,string ProductName,decimal TotalRevenue,decimal Price,int QuantitySold)
        {
            this.ProductID = ProductID;
            this.ProductName = ProductName;
            this.TotalRevenue = TotalRevenue;
            this.Price = Price;
            this.QuantitySold = QuantitySold;
        }
    }
}
