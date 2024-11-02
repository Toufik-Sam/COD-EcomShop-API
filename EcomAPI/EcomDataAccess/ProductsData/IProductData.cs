
namespace EcomDataAccess.ProductsData
{
    public interface IProductData
    {
        Task<int> AddProduct(ProductDTO Product);
        Task<bool> DeleteProduct(int ProductID);
        Task<IList<ProductDTO>> GetAllProducts();
        Task<ProductDTO> GetProductByID(int ProductID);
        Task<bool> UpdateProduct(ProductDTO Product);
        Task<int> IsProductExist(int ProductID);
    }
}