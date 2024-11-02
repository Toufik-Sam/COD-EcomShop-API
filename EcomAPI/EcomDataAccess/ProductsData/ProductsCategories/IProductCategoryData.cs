
namespace EcomDataAccess.ProductsData.ProductsCategories
{
    public interface IProductCategoryData
    {
        Task<int> AddProductCategories(ProductCategoryDTO ProductCategory);
        Task<IList<ProductCategoryDTO>> GetProductCategoriesByID(int ProductID);
        Task<bool> UpdateProductCategories(ProductCategoryDTO ProductCategory);
        Task<ProductCategoryDTO> GetProductCategoryByID(int ProductCategoryID);
    }
}