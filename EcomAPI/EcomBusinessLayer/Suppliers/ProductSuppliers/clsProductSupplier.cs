using EcomDataAccess.SupplierData.ProductSupplierData;

namespace EcomBusinessLayer.Suppliers.ProductSuppliers
{
    public class clsProductSupplier : IProductSupplier
    {
        private readonly IProductSupplierData _productSupplierData;

        public clsProductSupplier(IProductSupplierData productSupplierData)
        {
            this._productSupplierData = productSupplierData;
        }
        public async Task<ProductSupplierDTO> AddNewProductSupplier(ProductSupplierDTO productSupplier)
        {
            int newID = await _productSupplierData.AddNewProductSupplier(productSupplier);
            return newID != -1 ? new ProductSupplierDTO(newID, productSupplier.ProductID, productSupplier.SupplierID) : null!;
        }
        public async Task<bool> UpdateProductSupplier(ProductSupplierDTO productSupplier)
        {
            bool flag = await _productSupplierData.UpdateProductSupplier(productSupplier);
            return flag;
        }
        public async Task<IList<ProductSupplierDTO>> FindProductSuppliers(int ProductID)
        {
            var ProductSuppliersList = await _productSupplierData.GetProductSuppliers(ProductID);
            return ProductSuppliersList;
        }
        public async Task<ProductSupplierDTO>GetProductSupplierByID(int ID)
        {
            var productSupplier = await _productSupplierData.GetProductSupplierByID(ID);
            return productSupplier;
        }
        public async Task<bool> DeleteProductSupplier(int ProductSupplierID)
        {
            bool flag = await _productSupplierData.DeleteProductSupplier(ProductSupplierID);
            return flag;
        }
        public async Task<bool> IsProductSupplierExist(int ProductSupplierID)
        {
            int Found = await _productSupplierData.IsProductSupplierExist(ProductSupplierID);
            return Found != -1;
        }
    }
}
