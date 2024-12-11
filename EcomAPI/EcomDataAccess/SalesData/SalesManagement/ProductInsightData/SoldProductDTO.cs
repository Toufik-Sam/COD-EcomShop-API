namespace EcomDataAccess.SalesData.SalesManagement.ProductInsight
{
    public class SoldProductDTO
    {
        public int SoldProductID { get; set; }
        public string SoldProductName { get; set; }
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime LastSoldDate { get; set; }
        public SoldProductDTO(int SoldProductID, string SoldProductName, int TotalQuantitySold, decimal TotalRevenue, decimal UnitPrice,
            DateTime LastSoldDate)
        {
            this.SoldProductID = SoldProductID;
            this.SoldProductName = SoldProductName;
            this.TotalQuantitySold = TotalQuantitySold;
            this.TotalRevenue = TotalRevenue;
            this.UnitPrice = UnitPrice;
            this.LastSoldDate = LastSoldDate;
        }
    }
}
