
namespace EcomDataAccess.SupplierData.ProductSupplierData
{
    public interface IProductSupplierData
    {
        Task<int> AddNewProductSupplier(ProductSupplierDTO productSupplier);
        Task<bool> DeleteProductSupplier(int ProductSupplierID);
        Task<IList<ProductSupplierDTO>> GetProductSuppliers(int ProductID);
        Task<bool> UpdateProductSupplier(ProductSupplierDTO productSupplier);
        Task<int> IsProductSupplierExist(int ProductSupplierID);
        Task<ProductSupplierDTO> GetProductSupplierByID(int ProductSupplierID);
    }
}