
namespace EcomDataAccess.ProductsData.Galleries
{
    public interface IGalleryData
    {
        Task<int> AddNewGallery(GalleryDTO gelleryDTO);
        Task<IList<GalleryDTO>> GetProductGalleries(int ProductID);
        Task<GalleryDTO> GetProductGalleryByID(int GalleryID);
        Task<bool> UpdateGallery(GalleryDTO galleryDTO);
    }
}