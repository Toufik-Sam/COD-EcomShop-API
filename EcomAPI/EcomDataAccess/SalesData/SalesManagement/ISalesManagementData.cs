
namespace EcomDataAccess.SalesData.SalesManagement
{
    public interface ISalesManagementData
    {
        Task<DailySalesSummaryDTO> GetDailySaleSummary(DateTime Date);
        Task<MonthlySaleTrendDTO> GetMonthlySalesTrends(int year, int month);
        Task<SaleByRegionDTO> GetSaleByRegion(SalesByRegionArgs Args);
    }
}