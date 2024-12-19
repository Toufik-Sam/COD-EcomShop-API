using EcomDataAccess.SupplierData.ProductSupplierData;

namespace EcomBusinessLayer.Suppliers.ProductSuppliers
{
    public interface IProductSupplier
    {
        Task<ProductSupplierDTO> AddNewProductSupplier(ProductSupplierDTO productSupplier);
        Task<bool> DeleteProductSupplier(int ProductSupplierID);
        Task<IList<ProductSupplierDTO>> FindProductSuppliers(int ProductID);
        Task<bool> UpdateProductSupplier(ProductSupplierDTO productSupplier);
        Task<bool> IsProductSupplierExist(int ProductSupplierID);
        Task<ProductSupplierDTO> GetProductSupplierByID(int ID);
    }
}