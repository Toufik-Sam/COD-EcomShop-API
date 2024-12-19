namespace EcomDataAccess.SupplierData.ProductSupplierData
{
    public class ProductSupplierDTO
    {
        public int ProductSupplierID { get; set; }
        public int ProductID { get; set; }
        public int SupplierID { set; get; }
        public ProductSupplierDTO(int ProductSupplierID,int ProductID,int SupplierID)
        {
            this.ProductSupplierID = ProductSupplierID;
            this.ProductID = ProductID;
            this.SupplierID = SupplierID;
        }
    }
}
