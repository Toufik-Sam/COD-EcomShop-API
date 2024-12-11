using EcomDataAccess.SalesData.SalesManagement.CustomerBehavior;

namespace EcomBusinessLayer.Sales.SalesDetails.CustomerBehavior
{
    public interface ICustomerBehavior
    {
        Task<CustomerPurchaseHistoryDTO> GetCustomerPurchaseHistory(int CustomerID, DateTime?StartDate, DateTime?EndDate);
        Task<IList<TopCustomerDTO>> GetTopCustomer(int Top = -1);
        Task<IList<TopCustomerDTO>> GetTopCustomerByRange(DateTime?StartDate, DateTime?EndDate, int Top = -1);
    }
}