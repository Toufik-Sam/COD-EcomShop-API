using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcomDataAccess.CustomersData.CustomersAddresses;
using EcomDataAccess.ProductsData;

namespace EcomDataAccess.CustomersData.CustomersAddresses
{
    public class clsCustomerAddressData : ICustomerAddressData
    {
        private readonly IDataAccessSettings _settings;

        public clsCustomerAddressData(IDataAccessSettings settings)
        {
            this._settings = settings;
        }
        public async Task<int> AddCustomerAddress(CustomerAddressDTO customerDTO)
        {
            int NewAddressID = -1;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spAddCustomerAddress", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", customerDTO.CustomerID);
                    command.Parameters.AddWithValue("@addressLine1", customerDTO.AddressLine1);
                    command.Parameters.AddWithValue("@addressLine2", customerDTO.AddressLine2);
                    command.Parameters.AddWithValue("@Postal_Code", customerDTO.PostalCode);
                    command.Parameters.AddWithValue("@Country", customerDTO.Country);
                    command.Parameters.AddWithValue("@City", customerDTO.City);
                    command.Parameters.AddWithValue("@Phone_Number", customerDTO.PhoneNumber);

                    var OutputIdParam = new SqlParameter("@NewAddressID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(OutputIdParam);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                    NewAddressID = (int)OutputIdParam.Value;
                }
            }
            return NewAddressID;
        }
        public async Task<bool> UpdateCustomerAddress(CustomerAddressDTO customerDTO)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spUpdateCustomerAddress", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AddressID", customerDTO.AddressID);
                    command.Parameters.AddWithValue("CustomerID", customerDTO.CustomerID);
                    command.Parameters.AddWithValue("@addressLine1", customerDTO.AddressLine1);
                    command.Parameters.AddWithValue("@addressLine2", customerDTO.AddressLine2);
                    command.Parameters.AddWithValue("@Postal_Code", customerDTO.PostalCode);
                    command.Parameters.AddWithValue("@Country", customerDTO.Country);
                    command.Parameters.AddWithValue("@City", customerDTO.City);
                    command.Parameters.AddWithValue("@Phone_Number", customerDTO.PhoneNumber);
                    connection.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<CustomerAddressDTO> GetCustomerAddressByID(int AddressID)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetCustomerAddressByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AddressID", AddressID);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new CustomerAddressDTO(
                                 reader.GetInt32(reader.GetOrdinal("AddressID")),
                                 reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                 reader.GetString(reader.GetOrdinal("addressLine1")),
                                 reader.GetString(reader.GetOrdinal("addressLine2")),
                                 reader.GetString(reader.GetOrdinal("Postal_Code")),
                                 reader.GetString(reader.GetOrdinal("Country")),
                                 reader.GetString(reader.GetOrdinal("City")),
                                 reader.GetString(reader.GetOrdinal("Phone_Number"))
                                 );
                        }
                    }
                }
            }
            return null;
        }
        public async Task<IList<CustomerAddressDTO>> GetAllCustomerAddresses(int CustomerID)
        {
            var CustomerAddressesList = new List<CustomerAddressDTO>();
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetAllCustomerAddresses", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", CustomerID);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            CustomerAddressesList.Add(new CustomerAddressDTO(
                                reader.GetInt32(reader.GetOrdinal("AddressID")),
                                 reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                 reader.GetString(reader.GetOrdinal("addressLine1")),
                                 reader.GetString(reader.GetOrdinal("addressLine2")),
                                 reader.GetString(reader.GetOrdinal("Postal_Code")),
                                 reader.GetString(reader.GetOrdinal("Country")),
                                 reader.GetString(reader.GetOrdinal("City")),
                                 reader.GetString(reader.GetOrdinal("Phone_Number"))));
                        }
                        return CustomerAddressesList;
                    }
                }
            }
            return null;
        }
        public async Task<bool> DeleteCustomerAddress(int AddressID)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spDeleteCustomerAddress", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AddressID", AddressID);
                    conn.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected>0;
        }
        public async Task<int>IsCustomerExist(int CustomerID)
        {
            int Found = -1;
            using (SqlConnection connection = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spIsCustomerExist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", CustomerID);
                    connection.Open();
                    object result = await command.ExecuteScalarAsync();
                    if (result != null && int.TryParse(result.ToString(), out int ID))
                        Found = ID;
                }
            }
            return Found;
        }
    }
}
