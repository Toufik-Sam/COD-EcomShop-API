using EcomDataAccess.ProductsData.CategoriesData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomBusinessLayer.Products.Categories
{
    public class clsCategory : ICategory
    {
        private readonly ICategoryData _catgoryData;

        public clsCategory(ICategoryData catgoryData)
        {
            this._catgoryData = catgoryData;
        }
        public async Task<CategoryDTO> AddNewCategory(CategoryDTO category)
        {
            int newID = await _catgoryData.AddCategory(category);
            return newID > 0 ? new CategoryDTO(newID, category.EmployeeID, category.CategoryName, category.CategoryDescription) : null!;
        }
        public async Task<CategoryDTO> Find(int categoryID)
        {
            CategoryDTO category = await _catgoryData.GetCategoryByID(categoryID);
            return category;
        }
        public async Task<bool> UpdateCategory(CategoryDTO category)
        {
            bool flag = await _catgoryData.UpdateCategory(category);
            return flag;
        }
        public async Task<IList<CategoryDTO>> GetAllCategories()
        {
            var CategoriesList = await _catgoryData.GetAllCategories();
            return CategoriesList;
        }
        public async Task<bool> DeleteCategory(int CategoryID)
        {
            bool flag = await _catgoryData.DeleteCategory(CategoryID);
            return flag;
        }
        public async Task<bool>IsCategoryID(int CategoryID)
        {
            int Found = await _catgoryData.IsCategoryExist(CategoryID);
            return Found != -1;
        }
    }
}
