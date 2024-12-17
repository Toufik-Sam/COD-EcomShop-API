using EcomDataAccess.SalesData;

namespace EcomBusinessLayer.Sales
{
    public interface ISale
    {
        Task<SaleDTO> AddNewSale(SaleDTO saleDTO);
        Task<bool> DeleteSale(int SaleID);
        Task<SaleDTO> Find(int SaleID);
        Task<FullSaleDTO> FindFull(int SaleID);
        Task<IList<FullSaleDTO>> GetAllFullSales();
        Task<IList<SaleDTO>> GetAllSales();
        Task<bool> IsSaleExist(int SaleID);
        Task<bool> UpdateSale(SaleDTO saleDTO);
    }
}