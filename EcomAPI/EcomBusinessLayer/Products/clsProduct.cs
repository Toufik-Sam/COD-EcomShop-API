using EcomDataAccess.ProductsData;
using EcomDataAccess.ProductsData.Galleries;
using EcomDataAccess.ProductsData.ProductsCategories;
namespace EcomBusinessLayer.Products
{
    public class clsProduct : IProduct
    {
        private readonly IProductData _productData;
        private readonly IProductCategoryData _productCategoryData;
        private readonly IGalleryData _galleryData;

        public clsProduct(IProductData productData, IProductCategoryData productCategoryData,IGalleryData galleryData)
        {
            _productData = productData;
            _productCategoryData = productCategoryData;
            _galleryData = galleryData;
        }
        public async Task<ProductDTO> AddNewProduct(ProductDTO product)
        {
            int newID = await _productData.AddProduct(product);
            return newID > 0 ? new ProductDTO(newID, product.EmployeeID,product.SupplierID, product.ProductName, product.ProductDescription,
                product.Price, product.Quantity, product.Created_At) : null!;
        }
        public async Task<ProductDTO> Find(int ProductID)
        {
            ProductDTO Res = await _productData.GetProductByID(ProductID);
            return Res;
        }
        public async Task<bool> UpdateProduct(ProductDTO product)
        {
            bool flag = await _productData.UpdateProduct(product);
            return flag;
        }
        public async Task<IList<ProductDTO>> GetAllProducts()
        {
            var ProductsList = await _productData.GetAllProducts();
            return ProductsList;
        }
        public async Task<bool> DeleteProduct(int ProductID)
        {
            bool flag = await _productData.DeleteProduct(ProductID);
            return flag;
        }
        public async Task<IList<ProductCategoryDTO>>GetProductCategories(int ProductID)
        {
            var ProductCategories = await _productCategoryData.GetProductCategoriesByID(ProductID);
            return ProductCategories;
        }
        public async Task<ProductCategoryDTO>AddNewProductCategories(ProductCategoryDTO productCategory)
        {
            int newID = await _productCategoryData.AddProductCategories(productCategory);
            return newID > 0 ? new ProductCategoryDTO(newID, productCategory.ProductID, productCategory.CategoryID) : null!;
        }
        public async Task<bool>UpdateProductCategory(ProductCategoryDTO productCategory)
        {
            bool flag = await _productCategoryData.UpdateProductCategories(productCategory);
            return flag;
        }
        public async Task<ProductCategoryDTO>FindProductCategoryByID(int ProductCategoryByID)
        {
            ProductCategoryDTO Res = await _productCategoryData.GetProductCategoryByID(ProductCategoryByID);
            return Res;
        }
        public async Task<GalleryDTO>AddNewProductImage(GalleryDTO galleryDTO)
        {
            int newID =await _galleryData.AddNewGallery(galleryDTO);
            return newID > 0 ? new GalleryDTO(newID, galleryDTO.ProductID, galleryDTO.ImagePath) : null!;
        }
        public async Task<bool>UpdateProductImage(GalleryDTO galleryDTO)
        {
            bool flag = await _galleryData.UpdateGallery(galleryDTO);
            return flag;
        }
        public async Task<GalleryDTO> GetProductGalleryByID(int GalleryID)
        {
            GalleryDTO Res = await _galleryData.GetProductGalleryByID(GalleryID);
            return Res;
        }
        public async Task<IList<GalleryDTO>> GetProductGalleries(int ProductID)
        {
            var GalleriesList = await _galleryData.GetProductGalleries(ProductID);
            return GalleriesList;
        }
        public async Task<bool>DeleteProductGallery(int GalleryID)
        {
            bool flag = await _productData.DeleteProduct(GalleryID);
            return flag;
        }
        public async Task<bool>IsProductExist(int ProductID)
        {
            int Found = await _productData.IsProductExist(ProductID);
            return Found != -1;
        }
    }
}
