using EcomDataAccess.CustomersData;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.EmployeesData
{
    public class clsEmployeeData:IEmployeeData
    {
        private readonly IDataAccessSettings _settings;
        public clsEmployeeData(IDataAccessSettings settings)
        {
            this._settings = settings;
        }
        public async Task<int>AddEmployee(FullEmployeeDTO Employee)
        {
            int NewEmployeeID = -1;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spAddNewEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FirstName", Employee.FirstName);
                    command.Parameters.AddWithValue("@LastName", Employee.LastName);
                    command.Parameters.AddWithValue("@Email", Employee.Email);
                    command.Parameters.AddWithValue("@Phone", Employee.Phone);
                    command.Parameters.AddWithValue("@Password_Hash", Employee.Password);
                    command.Parameters.AddWithValue("Registered_At", Employee.Registered_At);
                    command.Parameters.AddWithValue("@PermissionLevel", Employee.PermissionLevel);
                    var OutputIdParam = new SqlParameter("@NewEmployeeID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(OutputIdParam);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                    NewEmployeeID = (int)OutputIdParam.Value;
                }
            }
            return NewEmployeeID;
        }
        public async Task<bool> UpdateEmployee(EmployeeDTO employee)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spUpdateEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);
                    command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    command.Parameters.AddWithValue("@LastName", employee.LastName);
                    command.Parameters.AddWithValue("@Email", employee.Email);
                    command.Parameters.AddWithValue("@Phone", employee.Phone);
                    command.Parameters.AddWithValue("Registered_At", employee.Registered_At);
                    command.Parameters.AddWithValue("@PermissionLevel", employee.PermissionLevel);
                    connection.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<EmployeeDTO> GetEmployeeByID(int EmployeeID)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetEmployeeByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new EmployeeDTO(
                                 reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                 reader.GetString(reader.GetOrdinal("FirstName")),
                                 reader.GetString(reader.GetOrdinal("LastName")),
                                 reader.GetString(reader.GetOrdinal("Email")),
                                 reader.GetString(reader.GetOrdinal("Phone")),
                                 reader.GetDateTime(reader.GetOrdinal("Registered_At")),
                                 (Permissions)reader.GetInt32(reader.GetOrdinal("PermissionLevel"))
                                 );
                        }
                    }
                }
            }
            return null;
        }
        public async Task<EmployeeDTO> GetEmployeeByEmailAndPassword(string Email, string Password)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetEmployeeByEmailAndPassword", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Password", Password);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new EmployeeDTO(
                                 reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                 reader.GetString(reader.GetOrdinal("FirstName")),
                                 reader.GetString(reader.GetOrdinal("LastName")),
                                 reader.GetString(reader.GetOrdinal("Email")),
                                 reader.GetString(reader.GetOrdinal("Phone")),
                                 reader.GetDateTime(reader.GetOrdinal("Registered_At")),
                                 (Permissions)reader.GetInt32(reader.GetOrdinal("PermissionLevel"))
                                 );
                        }
                    }
                }
            }
            return null;
        }
        public async Task<IList<EmployeeDTO>> GetAllEmployees()
        {
            var EmployeesList = new List<EmployeeDTO>();
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spGetAllEmployees", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            EmployeesList.Add(new EmployeeDTO(
                                 reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                 reader.GetString(reader.GetOrdinal("FirstName")),
                                 reader.GetString(reader.GetOrdinal("LastName")),
                                 reader.GetString(reader.GetOrdinal("Email")),
                                 reader.GetString(reader.GetOrdinal("Phone")),
                                 reader.GetDateTime(reader.GetOrdinal("Registered_At")),
                                 (Permissions)reader.GetInt32(reader.GetOrdinal("PermissionLevel"))
                                 ));
                        }
                    }
                }
            }
            return EmployeesList;
        }
        public async Task<bool> DeleteEmployee(int EmployeeID)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spDeleteEmployee", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                    conn.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected == 1;
        }
        public async Task<int>IsEmployeeExist(int EmployeeID)
        {
            int Found = -1;
            using (SqlConnection connection = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spIsEmployeeExist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                    connection.Open();
                    object result = await command.ExecuteScalarAsync();
                    if (result != null && int.TryParse(result.ToString(), out int ID))
                        Found = ID;
                }
            }
            return Found;
        }
        public async Task<bool> UpdatePassword(int EmployeeID, string CurrentPassword, string NewPassword)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spUpdateEmployeePassword", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeID", EmployeeID);
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
