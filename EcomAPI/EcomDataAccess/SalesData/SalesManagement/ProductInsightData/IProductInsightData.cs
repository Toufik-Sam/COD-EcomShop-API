
namespace EcomDataAccess.SalesData.SalesManagement.ProductInsight
{
    public interface IProductInsightData
    {
        Task<IList<SoldProductDTO>> GetSoldProductsList();
        Task<IList<SoldProductDTO>> GetTopSellingProductsListByDateRange(DateTime?StartDate, DateTime?EndDate, int Top = -1);
    }
}