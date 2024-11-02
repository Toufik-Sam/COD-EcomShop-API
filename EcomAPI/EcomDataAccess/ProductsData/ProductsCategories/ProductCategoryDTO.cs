using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.ProductsData.ProductsCategories
{
    public class ProductCategoryDTO
    {
        public int ProductCategoryID { set; get; }
        public int ProductID { set; get; }
        public int CategoryID { set; get; }
        public ProductCategoryDTO(int ProductCategoryID, int ProductID, int CategoryID)
        {
            this.ProductCategoryID = ProductCategoryID;
            this.ProductID = ProductID;
            this.CategoryID = CategoryID;
        }
    }
}
