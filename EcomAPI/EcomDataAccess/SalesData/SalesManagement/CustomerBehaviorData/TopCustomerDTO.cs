namespace EcomDataAccess.SalesData.SalesManagement.CustomerBehavior
{
    public class TopCustomerDTO
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal TotalAmountSpent { get; set; }
        public int TotalOrders { get; set; }
        public DateTime LastPurchaseDate { get; set; }
        public TopCustomerDTO(int CustomerID, string FirstName, string LastName, decimal TotalAmountSpent, int TotalOrders,
            DateTime LastPurchaseDate)
        {
            this.CustomerID = CustomerID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.TotalAmountSpent = TotalAmountSpent;
            this.TotalOrders = TotalOrders;
            this.LastPurchaseDate = LastPurchaseDate;
        }
    }
}
