
namespace EcomDataAccess.SalesData.SalesManagement.CustomerBehavior
{
    public interface ICustomerBehaviorData
    {
        Task<CustomerPurchaseHistoryDTO> GetCustomerPurchaseHistory(int CustomerID, DateTime?StartDate, DateTime?EndDate);
        Task<IList<TopCustomerDTO>> GetTopCustomers(int Top = -1);
        Task<IList<TopCustomerDTO>> GetTopCustomersByRange(DateTime?StartDate, DateTime?EndDate, int Top = -1);
    }
}