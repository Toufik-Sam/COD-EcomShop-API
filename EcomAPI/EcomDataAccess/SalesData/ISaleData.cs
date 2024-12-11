
namespace EcomDataAccess.SalesData
{
    public interface ISaleData
    {
        Task<int> AddNewSale(SaleDTO Sale);
        Task<bool> DeleteSale(int SaleID);
        Task<IList<SaleDTO>> GetAllSales();
        Task<IList<FullSaleDTO>> GetAllFullSales();
        Task<SaleDTO> GetSaleByID(int SaleID);
        Task<FullSaleDTO> GetFullSaleByID(int SaleID);
        Task<int> IsSaleExit(int SaleID);
        Task<bool> UpdateSale(SaleDTO Sale);
    }
}