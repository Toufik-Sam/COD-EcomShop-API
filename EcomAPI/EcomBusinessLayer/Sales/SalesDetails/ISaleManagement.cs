using EcomDataAccess.SalesData.SalesManagement;

namespace EcomBusinessLayer.Sales.SalesDetails
{
    public interface ISaleManagement
    {
        Task<DailySalesSummaryDTO> GetDailySaleSummary(DateTime Date);
        Task<MonthlySaleTrendDTO> GetMonthlySalesTrends(int year, int month);
        Task<SaleByRegionDTO> GetSaleByRegion(SalesByRegionArgs Args);
    }
}