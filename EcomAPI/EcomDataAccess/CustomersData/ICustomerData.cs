
namespace EcomDataAccess.CustomersData
{
    public interface ICustomerData
    {
        Task<int> AddCustomer(FullCustomerDTO CustomerDTO);
        Task<bool> DeleteCustomer(int CustomerID);
        Task<IList<CustomerDTO>> GetAllCustomers();
        Task<CustomerDTO> GetCustomerByID(int CustomerID);
        Task<bool> UpdateCustomer(CustomerDTO CustomerDTO);
        Task<CustomerDTO> GetCustomerByEmailAndPassword(string Email, string Password);
        Task<int> IsCustomerExist(int CustomerID);
        Task<bool> UpdatePassword(int CustomerID, string CurrentPassword, string NewPassword);
    }
}