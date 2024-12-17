namespace EcomDataAccess.SalesData.SalesManagement
{
    public class OrderProductDTO
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int QuantityPurchased { get; set; }
        public decimal Price { get; set; }
        public OrderProductDTO(int ProductID,string ProductName,int QuantityPurchased,decimal Price)
        {
            this.ProductID = ProductID;
            this.ProductName = ProductName;
            this.QuantityPurchased = QuantityPurchased;
            this.Price = Price;
        }
    }
}
