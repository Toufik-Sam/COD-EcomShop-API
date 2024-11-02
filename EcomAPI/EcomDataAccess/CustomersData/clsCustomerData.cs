using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.CustomersData
{
    public class clsCustomerData : ICustomerData
    {
        private readonly IDataAccessSettings _settings;

        public clsCustomerData(IDataAccessSettings settings)
        {
            this._settings = settings;
        }
        public async Task<int> AddCustomer(FullCustomerDTO CustomerDTO)
        {
            int NewCustomerID = -1;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spAddNewCustomer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FirstName", CustomerDTO.FirstName);
                    command.Parameters.AddWithValue("@LastName", CustomerDTO.LastName);
                    command.Parameters.AddWithValue("@Email", CustomerDTO.Email);
                    command.Parameters.AddWithValue("@Phone", CustomerDTO.Phone);
                    command.Parameters.AddWithValue("@Password_Hash", CustomerDTO.Password);
                    command.Parameters.AddWithValue("Registered_At", CustomerDTO.Registered_At);
                    var OutputIdParam = new SqlParameter("@NewCustomerID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(OutputIdParam);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                    NewCustomerID = (int)OutputIdParam.Value;
                }
            }
            return NewCustomerID;
        }
        public async Task<bool> UpdateCustomer(CustomerDTO CustomerDTO)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spUpdateCustomer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", CustomerDTO.CustomerID);
                    command.Parameters.AddWithValue("@FirstName", CustomerDTO.FirstName);
                    command.Parameters.AddWithValue("@LastName", CustomerDTO.LastName);
                    command.Parameters.AddWithValue("@Email", CustomerDTO.Email);
                    command.Parameters.AddWithValue("@Phone", CustomerDTO.Phone);
                    command.Parameters.AddWithValue("Registered_At", CustomerDTO.Registered_At);
                    connection.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<CustomerDTO> GetCustomerByID(int CustomerID)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetCustomerByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", CustomerID);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new CustomerDTO(
                                 reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                 reader.GetString(reader.GetOrdinal("FirstName")),
                                 reader.GetString(reader.GetOrdinal("LastName")),
                                 reader.GetString(reader.GetOrdinal("Email")),
                                 reader.GetString(reader.GetOrdinal("Phone")),
                                 reader.GetDateTime(reader.GetOrdinal("Registered_At"))
                                 );
                        }
                    }
                }
            }
            return null;
        }
        public async Task<CustomerDTO>GetCustomerByEmailAndPassword(string Email,string Password)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetCustomerByEmailAndPassword", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Password", Password);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new CustomerDTO(
                                 reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                 reader.GetString(reader.GetOrdinal("FirstName")),
                                 reader.GetString(reader.GetOrdinal("LastName")),
                                 reader.GetString(reader.GetOrdinal("Email")),
                                 reader.GetString(reader.GetOrdinal("Phone")),
                                 reader.GetDateTime(reader.GetOrdinal("Registered_At"))
                                 );
                        }
                    }
                }
            }
            return null;
        }
        public async Task<IList<CustomerDTO>> GetAllCustomers()
        {
            var CustomersList = new List<CustomerDTO>();
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spGetAllCustomers", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            CustomersList.Add(new CustomerDTO(
                                 reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                 reader.GetString(reader.GetOrdinal("FirstName")),
                                 reader.GetString(reader.GetOrdinal("LastName")),
                                 reader.GetString(reader.GetOrdinal("Email")),
                                 reader.GetString(reader.GetOrdinal("Phone")),
                                 reader.GetDateTime(reader.GetOrdinal("Registered_At"))
                                 ));
                        }
                    }
                }
            }
            return CustomersList;
        }
        public async Task<bool> DeleteCustomer(int CustomerID)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spDeleteCustomer", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", CustomerID);
                    conn.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected == 1;
        }
        public async Task<int>IsCustomerExist(int CustomerID)
        {
            int Found = -1;
            using(SqlConnection connection=new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command=new SqlCommand("dbo.spIsCustomerExist",connection))
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
        public async Task<bool>UpdatePassword(int CustomerID,string CurrentPassword,string NewPassword)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spUpdateCustomerPassword", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", CustomerID);
                    command.Parameters.AddWithValue("@CurrentPassword", CurrentPassword);
                    command.Parameters.AddWithValue("@NewPassword", NewPassword);
                    connection.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
    }
}
