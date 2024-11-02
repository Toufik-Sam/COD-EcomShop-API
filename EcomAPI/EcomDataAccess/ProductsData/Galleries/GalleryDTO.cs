using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.ProductsData.Galleries
{
    public class GalleryDTO
    {
        public int GalleryID { set; get; }
        public int ProductID { set; get; }
        public string ImagePath { set; get; }
        public GalleryDTO(int GalleryID,int ProductID,string ImagePath)
        {
            this.GalleryID = GalleryID;
            this.ProductID = ProductID;
            this.ImagePath = ImagePath;
        }
    }
}
