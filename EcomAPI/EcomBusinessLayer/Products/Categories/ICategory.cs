using EcomDataAccess.ProductsData.CategoriesData;

namespace EcomBusinessLayer.Products.Categories
{
    public interface ICategory
    {
        Task<CategoryDTO> AddNewCategory(CategoryDTO category);
        Task<bool> DeleteCategory(int CategoryID);
        Task<CategoryDTO> Find(int categoryID);
        Task<IList<CategoryDTO>> GetAllCategories();
        Task<bool> UpdateCategory(CategoryDTO category);
        Task<bool> IsCategoryID(int CategoryID);
    }
}