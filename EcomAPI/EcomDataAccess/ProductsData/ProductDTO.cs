using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.ProductsData
{
    public class ProductDTO
    {
        public int ProductID { set; get; }
        public int EmployeeID { set; get; }
        public string ProductName { set; get; }
        public string ProductDescription { set; get; }
        public decimal Price { set; get; }
        public int Quantity { set; get; }
        public DateTime Created_At { set; get; }
        public ProductDTO(int ProductID,int EmployeeID,string ProductName,string ProductDescription,
            decimal Price,int Quantity,DateTime Created_At)
        {
            this.ProductID = ProductID;
            this.EmployeeID = EmployeeID;
            this.ProductName = ProductName;
            this.ProductDescription = ProductDescription;
            this.Price = Price;
            this.Quantity = Quantity;
            this.Created_At = Created_At;
        }

    }
}
