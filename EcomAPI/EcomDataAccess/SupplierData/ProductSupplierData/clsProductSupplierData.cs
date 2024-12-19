using System.Data.SqlClient;
using System.Data;

namespace EcomDataAccess.SupplierData.ProductSupplierData
{
    public class clsProductSupplierData : IProductSupplierData
    {
        private readonly IDataAccessSettings _setting;

        public clsProductSupplierData(IDataAccessSettings setting)
        {
            this._setting = setting;
        }
        public async Task<int> AddNewProductSupplier(ProductSupplierDTO productSupplier)
        {
            int NewProductSupplierID = -1;
            using (var connection = new SqlConnection(_setting.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spAddNewProductSupplier", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID", productSupplier.ProductID);
                    command.Parameters.AddWithValue("@SupplierID", productSupplier.SupplierID);
                    var OutputIdParam = new SqlParameter("@NewProductSupplierID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(OutputIdParam);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                    NewProductSupplierID = (int)OutputIdParam.Value;
                }
            }
            return NewProductSupplierID;
        }
        public async Task<bool> UpdateProductSupplier(ProductSupplierDTO productSupplier)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_setting.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spUpdateProductSupplier", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductSupplierID", productSupplier.ProductSupplierID);
                    command.Parameters.AddWithValue("@ProductID", productSupplier.ProductID);
                    command.Parameters.AddWithValue("@SupplierID", productSupplier.SupplierID);
                    connection.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<bool> DeleteProductSupplier(int ProductSupplierID)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_setting.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spDeleteProductSupplier", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductSupplierID", ProductSupplierID);
                    connection.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<IList<ProductSupplierDTO>> GetProductSuppliers(int ProductID)
        {
            var ProductSupplierList = new List<ProductSupplierDTO>();
            using (var connection = new SqlConnection(_setting.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetProductSuppliers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID", ProductID);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            ProductSupplierList.Add(new ProductSupplierDTO(
                                reader.GetInt32(reader.GetOrdinal("ProductSupplierID")),
                                reader.GetInt32(reader.GetOrdinal("ProductID")),
                                reader.GetInt32(reader.GetOrdinal("SupplierID"))
                                ));
                        }
                    }
                }
            }
            return ProductSupplierList;
        }
        public async Task<ProductSupplierDTO>GetProductSupplierByID(int ProductSupplierID)
        {
            
            using (var connection = new SqlConnection(_setting.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetProductSupplierByID", connection))
                {
                    command.Parameters.AddWithValue("@ProductSupplierID", ProductSupplierID);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                            return new ProductSupplierDTO(
                                reader.GetInt32(reader.GetOrdinal("ProductSupplierID")),
                                reader.GetInt32(reader.GetOrdinal("ProductID")),
                                reader.GetInt32(reader.GetInt32("SupplierID"))
                                );
                    }
                }
            }
            return null;
        }
        public async Task<int>IsProductSupplierExist(int ProductSupplierID)
        {
            int Found = -1;
            using (var connection = new SqlConnection(_setting.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spIsProductSupplierExist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductSupplierID", ProductSupplierID);
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
