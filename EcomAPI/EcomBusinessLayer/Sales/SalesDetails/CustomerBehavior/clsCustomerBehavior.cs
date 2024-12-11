using EcomDataAccess.SalesData.SalesManagement.CustomerBehavior;

namespace EcomBusinessLayer.Sales.SalesDetails.CustomerBehavior
{
    public class clsCustomerBehavior : ICustomerBehavior
    {
        private readonly ICustomerBehaviorData _customerBehaviorData;

        public clsCustomerBehavior(ICustomerBehaviorData CustomerBehaviorData)
        {
            this._customerBehaviorData = CustomerBehaviorData;
        }
        public async Task<IList<TopCustomerDTO>> GetTopCustomer(int Top = -1)
        {
            var TopCustomer = await _customerBehaviorData.GetTopCustomers(Top);
            return TopCustomer;
        }
        public async Task<IList<TopCustomerDTO>> GetTopCustomerByRange(DateTime?StartDate, DateTime?EndDate, int Top = -1)
        {
            var TopCustomerByRange = await _customerBehaviorData.GetTopCustomersByRange(StartDate, EndDate, Top);
            return TopCustomerByRange;
        }
        public async Task<CustomerPurchaseHistoryDTO> GetCustomerPurchaseHistory(int CustomerID, DateTime?StartDate, DateTime?EndDate)
        {
            var customerPurchaseHistory = await _customerBehaviorData.GetCustomerPurchaseHistory(CustomerID, StartDate, EndDate);
            return customerPurchaseHistory;
        }
    }
}
