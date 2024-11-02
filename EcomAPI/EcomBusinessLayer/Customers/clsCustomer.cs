using EcomDataAccess.CustomersData;
using EcomDataAccess.CustomersData.CustomersAddresses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EcomBusinessLayer.Customers
{
    public class clsCustomer : ICustomer
    {
        private readonly ICustomerData _customerData;
        private readonly ICustomerAddressData _customerAddressData;

        public clsCustomer(ICustomerData customerData,ICustomerAddressData customerAddressData)
        {
            this._customerData = customerData;
            this._customerAddressData = customerAddressData;
        }
        public async Task<CustomerDTO> AddNewCustomer(FullCustomerDTO customerDTO)
        {
            int newID = await _customerData.AddCustomer(customerDTO);
            return newID > 0 ? new CustomerDTO(newID, customerDTO.FirstName, customerDTO.LastName, customerDTO.Email,
                customerDTO.Phone, customerDTO.Registered_At) : null!;
        }
        public async Task<CustomerDTO> Find(int CustomerID)
        {
            CustomerDTO Res = await _customerData.GetCustomerByID(CustomerID);
            return Res;
        }
        public async Task<CustomerDTO>Find(string Email,string Password)
        {
            CustomerDTO Res = await _customerData.GetCustomerByEmailAndPassword(Email, Password);
            return Res;
        }
        public async Task<bool> UpdateCustomer(CustomerDTO customerDTO)
        {
            bool flag = await _customerData.UpdateCustomer(customerDTO);
            return flag;
        }
        public async Task<IList<CustomerDTO>> GetAllCustomers()
        {
            var CustomersList = await _customerData.GetAllCustomers();
            return CustomersList;
        }
        public async Task<bool> DeleteCustomer(int CustomerID)
        {
            bool flag = await _customerData.DeleteCustomer(CustomerID);
            return flag;
        }
        public async Task<CustomerAddressDTO>AddNewCustomerAddress(CustomerAddressDTO customerAddressDTO)
        {
            int NewID = await _customerAddressData.AddCustomerAddress(customerAddressDTO);
            return NewID > 0 ? new CustomerAddressDTO(NewID, customerAddressDTO.CustomerID, customerAddressDTO.AddressLine1,
                customerAddressDTO.AddressLine2, customerAddressDTO.PostalCode, customerAddressDTO.Country, customerAddressDTO.City
                , customerAddressDTO.PhoneNumber) : null!;
        }
        public async Task<bool>UpdateCustomerAddress(CustomerAddressDTO customerAddressDTO)
        {
            bool flag = await _customerAddressData.UpdateCustomerAddress(customerAddressDTO);
            return flag;
        }
        public async Task<IList<CustomerAddressDTO>>GetAllCustomerAddresses(int CustomerID)
        {
            var CustomerAddressesList = await _customerAddressData.GetAllCustomerAddresses(CustomerID);
            return CustomerAddressesList;
        }
        public async Task<CustomerAddressDTO>GetCustomerAddressByID(int AddressID)
        {
            CustomerAddressDTO Res = await _customerAddressData.GetCustomerAddressByID(AddressID);
            return Res;
        }
        public async Task<bool>DeleteCustomerAddressByID(int AddressID)
        {
            bool flag = await _customerAddressData.DeleteCustomerAddress(AddressID);
            return flag;
        }
        public async Task<bool>IsCustomerExist(int CustomerID)
        {
            int Found = await _customerData.IsCustomerExist(CustomerID);
            return Found!=-1;
        }
        public async Task<bool> UpdateCustomerPassword(int CustomerID, string CurrentPassword, string NewPassword)
        {
            bool flag =await _customerData.UpdatePassword(CustomerID, CurrentPassword, NewPassword);
            return flag;
        }
    }
}
