using EcomDataAccess.OrdersData.OrderItems;
using EcomDataAccess.SalesData;

namespace EcomBusinessLayer.Sales
{
    public class clsSale : ISale
    {
        private readonly ISaleData _saleData;
        private readonly IOrderItemsData _orderItemsData;
        public clsSale(ISaleData saleData, IOrderItemsData orderItemsData)
        {
            this._saleData = saleData;
            this._orderItemsData = orderItemsData;
        }
        public async Task<SaleDTO> AddNewSale(SaleDTO saleDTO)
        {
            int newID = await _saleData.AddNewSale(saleDTO);
            return newID > 0 ? new SaleDTO(newID, saleDTO.OrderID, saleDTO.Amount) : null!;
        }
        public async Task<bool> UpdateSale(SaleDTO saleDTO)
        {
            bool flag = await _saleData.UpdateSale(saleDTO);
            return flag;
        }
        public async Task<SaleDTO> Find(int SaleID)
        {
            var Res = await _saleData.GetSaleByID(SaleID);
            return Res;
        }
        public async Task<FullSaleDTO> FindFull(int SaleID)
        {
            var Res = await _saleData.GetFullSaleByID(SaleID);
            return Res;
        }
        public async Task<IList<SaleDTO>> GetAllSales()
        {
            var SalesList = await _saleData.GetAllSales();
            return SalesList;
        }
        public async Task<IList<FullSaleDTO>> GetAllFullSales()
        {
            var SalesList = await _saleData.GetAllFullSales();
            return SalesList;
        }
        public async Task<bool> DeleteSale(int SaleID)
        {
            bool flag = await _saleData.DeleteSale(SaleID);
            return flag;
        }
        public async Task<bool> IsSaleExist(int SaleID)
        {
            int Found = await _saleData.IsSaleExit(SaleID);
            return Found != -1;
        }
    }
}
