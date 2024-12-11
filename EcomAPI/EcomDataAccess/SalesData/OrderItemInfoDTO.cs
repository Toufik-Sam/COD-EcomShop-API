namespace EcomDataAccess.SalesData
{
    public class OrderItemInfoDTO
    {
        public int OrderItemID { set; get; }
        public int ProductID { set; get; }
        public string ProductName { set; get; }
        public decimal Price { set; get; }
        public int Quantity { set; get; }
        public OrderItemInfoDTO(int OrderItemID, int ProductID,string ProductName, decimal Price, int Quantity)
        {
            this.OrderItemID = OrderItemID;
            this.ProductID = ProductID;
            this.ProductName = ProductName;
            this.Price = Price;
            this.Quantity = Quantity;
        }
    }
}
