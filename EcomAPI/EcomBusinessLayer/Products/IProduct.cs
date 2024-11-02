using EcomDataAccess.ProductsData;
using EcomDataAccess.ProductsData.Galleries;
using EcomDataAccess.ProductsData.ProductsCategories;

namespace EcomBusinessLayer.Products
{
    public interface IProduct
    {
        Task<ProductDTO> AddNewProduct(ProductDTO product);
        Task<bool> DeleteProduct(int ProductID);
        Task<ProductDTO> Find(int ProductID);
        Task<IList<ProductDTO>> GetAllProducts();
        Task<bool> UpdateProduct(ProductDTO product);
        Task<IList<ProductCategoryDTO>> GetProductCategories(int ProductID);
        Task<ProductCategoryDTO> AddNewProductCategories(ProductCategoryDTO productCategory);
        Task<bool> UpdateProductCategory(ProductCategoryDTO productCategory);
        Task<ProductCategoryDTO> FindProductCategoryByID(int ProductCategoryByID);
        Task<GalleryDTO> AddNewProductImage(GalleryDTO galleryDTO);
        Task<bool> UpdateProductImage(GalleryDTO galleryDTO);
        Task<GalleryDTO> GetProductGalleryByID(int GalleryID);
        Task<IList<GalleryDTO>> GetProductGalleries(int ProductID);
        Task<bool> DeleteProductGallery(int GalleryID);
        Task<bool> IsProductExist(int ProductID);
    }
}