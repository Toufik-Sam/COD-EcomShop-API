using EcomDataAccess.SalesData.SalesManagement.ProductInsight;

namespace EcomBusinessLayer.Sales.SalesDetails.ProductInsight
{
    public interface IProductInsight
    {
        Task<IList<SoldProductDTO>> GetSoldProductsList();
        Task<IList<SoldProductDTO>> GetTopSellingProductsListByDateRange(DateTime?StartDate, DateTime?EndDate, int Top = -1);
    }
}