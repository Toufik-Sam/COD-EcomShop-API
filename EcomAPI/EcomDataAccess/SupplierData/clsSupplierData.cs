using System.Data;
using System.Data.SqlClient;

namespace EcomDataAccess.SupplierData
{
    public class clsSupplierData : ISupplierData
    {
        private readonly IDataAccessSettings _setting;

        public clsSupplierData(IDataAccessSettings setting)
        {
            this._setting = setting;
        }
        public async Task<int> AddNewSupplier(SupplierDTO supplierDTO)
        {
            int NewSupplierID = -1;
            using (var connection = new SqlConnection(_setting.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spAddNewSupplier", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SupplierID", supplierDTO.SupplierID);
                    command.Parameters.AddWithValue("@SupplierName", supplierDTO.SupplierName);
                    command.Parameters.AddWithValue("Address", supplierDTO.Address);
                    command.Parameters.AddWithValue("@Phone", supplierDTO.Phone);
                    var OutputIdParam = new SqlParameter("@NewSupplierID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(OutputIdParam);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                    NewSupplierID = (int)OutputIdParam.Value;
                }
            }
            return NewSupplierID;
        }
        public async Task<bool> UpdateSupplier(SupplierDTO supplierDTO)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_setting.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.UpdateSupplier", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SupplierID", supplierDTO.SupplierID);
                    command.Parameters.AddWithValue("@SupplierName", supplierDTO.SupplierName);
                    command.Parameters.AddWithValue("Address", supplierDTO.Address);
                    command.Parameters.AddWithValue("@Phone", supplierDTO.Phone);
                    connection.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<SupplierDTO> GetSupplierByID(int SupplierID)
        {
            using (var connection = new SqlConnection(_setting.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetSupplierByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SupplierID", SupplierID);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new SupplierDTO(
                                reader.GetInt32(reader.GetOrdinal("SupplierID")),
                                reader.GetString(reader.GetOrdinal("SupplierName")),
                                reader.GetString(reader.GetOrdinal("Address")),
                                reader.GetString(reader.GetOrdinal("Email")),
                                reader.GetString(reader.GetOrdinal("Phone"))
                                );
                        }
                    }
                }
            }
            return null;
        }
        public async Task<IList<SupplierDTO>> GetAllSuppliers()
        {
            var SupplierList = new List<SupplierDTO>();
            using (var connection = new SqlConnection(_setting.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetAllSuppliers", connection))
                {
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            SupplierList.Add(new SupplierDTO(
                                reader.GetInt32(reader.GetOrdinal("SupplierID")),
                                reader.GetString(reader.GetOrdinal("SupplierName")),
                                reader.GetString(reader.GetOrdinal("Address")),
                                reader.GetString(reader.GetOrdinal("Email")),
                                reader.GetString(reader.GetOrdinal("Phone"))
                                ));
                        }
                    }
                }
            }
            return SupplierList;
        }
        public async Task<bool> DeleteSupplier(int SupplierID)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_setting.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spDeleteSupplier", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SupplierID", SupplierID);
                    connection.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<int>IsSupplierExist(int SupplierID)
        {
            int Found = -1;
            using (var connection = new SqlConnection(_setting.ConnectionString))
            {
                using (var command=new SqlCommand("dbo.spIsSupplierExist",connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SupplierID", SupplierID);
                    connection.Open();
                    object res = await command.ExecuteScalarAsync();
                    if (res != null && int.TryParse(res.ToString(), out int ID))
                        Found = ID;
                }
            }
            return Found;
        }
    }
}
