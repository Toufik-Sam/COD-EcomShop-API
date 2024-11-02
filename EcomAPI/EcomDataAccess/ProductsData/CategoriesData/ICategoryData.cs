
namespace EcomDataAccess.ProductsData.CategoriesData
{
    public interface ICategoryData
    {
        Task<int> AddCategory(CategoryDTO category);
        Task<bool> DeleteCategory(int CategoryID);
        Task<IList<CategoryDTO>> GetAllCategories();
        Task<CategoryDTO> GetCategoryByID(int CatgeoryID);
        Task<bool> UpdateCategory(CategoryDTO category);
        Task<int> IsCategoryExist(int categoryID);
    }
}