using EcomDataAccess.CustomersData;
using EcomDataAccess.CustomersData.CustomersAddresses;

namespace EcomBusinessLayer.Customers
{
    public interface ICustomer
    {
        Task<CustomerDTO> AddNewCustomer(FullCustomerDTO customerDTO);
        Task<bool> DeleteCustomer(int CustomerID);
        Task<CustomerDTO> Find(int CustomerID);
        Task<IList<CustomerDTO>> GetAllCustomers();
        Task<bool> UpdateCustomer(CustomerDTO customerDTO);
        Task<CustomerDTO> Find(string Email, string Password);
        Task<CustomerAddressDTO> AddNewCustomerAddress(CustomerAddressDTO customerAddressDTO);
        Task<bool> UpdateCustomerAddress(CustomerAddressDTO customerAddressDTO);
        Task<IList<CustomerAddressDTO>> GetAllCustomerAddresses(int CustomerID);
        Task<CustomerAddressDTO> GetCustomerAddressByID(int AddressID);
        Task<bool> DeleteCustomerAddressByID(int AddressID);
        Task<bool> IsCustomerExist(int CustomerID);
        Task<bool> UpdateCustomerPassword(int CustomerID, string CurrentPassword, string NewPassword);
    }
}