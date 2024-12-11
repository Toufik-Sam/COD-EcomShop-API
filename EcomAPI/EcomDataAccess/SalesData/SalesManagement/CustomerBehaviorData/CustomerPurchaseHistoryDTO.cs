namespace EcomDataAccess.SalesData.SalesManagement.CustomerBehavior
{
    public class CustomerPurchaseHistoryDTO
    {
        public int CustomerID { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public IList<CustomerOrderDTO> Orders { set; get; }
        public CustomerPurchaseHistoryDTO(int CustomerID, string CustomerFirstName, string CustomerLastName,IList<CustomerOrderDTO> Orders)
        {
            this.CustomerID = CustomerID;
            this.CustomerFirstName = CustomerFirstName;
            this.CustomerLastName = CustomerLastName;
            this.Orders = Orders;
        }
    }
}
