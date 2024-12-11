using EcomDataAccess.SalesData.SalesManagement;

namespace EcomBusinessLayer.Sales.SalesDetails
{
    public class clsSaleManagement : ISaleManagement
    {
        private readonly ISalesManagementData _saleManagementData;

        public clsSaleManagement(ISalesManagementData saleManagementData)
        {
            this._saleManagementData = saleManagementData;
        }
        public async Task<SaleByRegionDTO> GetSaleByRegion(SalesByRegionArgs Args)
        {
            var RegionSaleData = await _saleManagementData.GetSaleByRegion(Args);
            return RegionSaleData;
        }
        public async Task<DailySalesSummaryDTO> GetDailySaleSummary(DateTime Date)
        {
            var DaySalesSummary = await _saleManagementData.GetDailySaleSummary(Date);
            return DaySalesSummary;
        }
        public async Task<MonthlySaleTrendDTO> GetMonthlySalesTrends(int year, int month)
        {
            var MonthSalesTrend = await _saleManagementData.GetMonthlySalesTrends(year, month);
            return MonthSalesTrend;
        }
    }
}
