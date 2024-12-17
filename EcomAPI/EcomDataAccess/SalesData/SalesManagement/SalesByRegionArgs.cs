namespace EcomDataAccess.SalesData.SalesManagement
{
    public class SalesByRegionArgs
    {
        public string Country { get; set; }
        public string City { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
