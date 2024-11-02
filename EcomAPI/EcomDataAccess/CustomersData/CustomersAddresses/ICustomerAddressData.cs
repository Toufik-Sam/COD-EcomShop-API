
namespace EcomDataAccess.CustomersData.CustomersAddresses
{
    public interface ICustomerAddressData
    {
        Task<int> AddCustomerAddress(CustomerAddressDTO customerDTO);
        Task<bool> DeleteCustomerAddress(int AddressID);
        Task<IList<CustomerAddressDTO>> GetAllCustomerAddresses(int CustomerID);
        Task<CustomerAddressDTO> GetCustomerAddressByID(int AddressID);
        Task<bool> UpdateCustomerAddress(CustomerAddressDTO customerDTO);
        Task<int> IsCustomerExist(int CustomerID);
    }
}