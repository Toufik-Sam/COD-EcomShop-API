namespace EcomDataAccess.SalesData.SalesManagement.CustomerBehavior
{
    public class CustomerOrderDTO
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public IList<OrderProductDTO> Products { get; set; }
        public CustomerOrderDTO(int OrderID, DateTime OrderDate, decimal OrderTotal, IList<OrderProductDTO> Products)
        {
            this.OrderID = OrderID;
            this.OrderDate = OrderDate;
            this.OrderTotal = OrderTotal;
            this.Products = Products;
        }
    }
}
