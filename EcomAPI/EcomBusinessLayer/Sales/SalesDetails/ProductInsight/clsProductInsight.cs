using EcomDataAccess.SalesData.SalesManagement.ProductInsight;

namespace EcomBusinessLayer.Sales.SalesDetails.ProductInsight
{
    public class clsProductInsight : IProductInsight
    {
        private readonly IProductInsightData _productInsightData;

        public clsProductInsight(IProductInsightData productInsightData)
        {
            this._productInsightData = productInsightData;
        }
        public async Task<IList<SoldProductDTO>> GetSoldProductsList()
        {
            var SoldProductsList = await _productInsightData.GetSoldProductsList();
            return SoldProductsList;
        }
        public async Task<IList<SoldProductDTO>> GetTopSellingProductsListByDateRange(DateTime?StartDate, DateTime?EndDate, int Top = -1)
        {
            var TopSellingProductsListByRange = await _productInsightData.GetTopSellingProductsListByDateRange(StartDate, EndDate, Top);
            return TopSellingProductsListByRange;
        }
    }
}
