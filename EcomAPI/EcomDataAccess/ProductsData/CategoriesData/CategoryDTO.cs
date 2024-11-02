using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.ProductsData.CategoriesData
{
    public class CategoryDTO
    {
        public int CategoryID { set; get; }
        public int EmployeeID { set; get; }
        public string CategoryName { set; get; }
        public string CategoryDescription { set; get; }
        public CategoryDTO(int CategoryID,int EmployeeID,string CategoryName,string CategoryDescription)
        {
            this.CategoryID = CategoryID;
            this.EmployeeID = EmployeeID;
            this.CategoryName = CategoryName;
            this.CategoryDescription = CategoryDescription;
        }

    }
}
