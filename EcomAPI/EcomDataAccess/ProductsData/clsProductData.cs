using EcomDataAccess.CustomersData;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.ProductsData
{
    public class clsProductData : IProductData
    {
        private readonly IDataAccessSettings _settings;
        public clsProductData(IDataAccessSettings settings)
        {
            this._settings = settings;
        }
        public async Task<int> AddProduct(ProductDTO Product)
        {
            int NewProduct = -1;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spAddNewProduct", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeID", Product.EmployeeID);
                    command.Parameters.AddWithValue("@ProductName", Product.ProductName);
                    command.Parameters.AddWithValue("@ProductDescription", Product.ProductDescription);
                    command.Parameters.AddWithValue("@Price", Product.Price);
                    command.Parameters.AddWithValue("@Quantity", Product.Quantity);
                    command.Parameters.AddWithValue("@Created_At", Product.Created_At);

                    var OutputIdParam = new SqlParameter("@NewProductID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(OutputIdParam);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                    NewProduct = (int)OutputIdParam.Value;
                }
            }
            return NewProduct;
        }
        public async Task<bool> UpdateProduct(ProductDTO Product)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spUpdateProduct", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID", Product.ProductID);
                    command.Parameters.AddWithValue("@EmployeeID", Product.EmployeeID);
                    command.Parameters.AddWithValue("@ProductName", Product.ProductName);
                    command.Parameters.AddWithValue("@ProductDescription", Product.ProductDescription);
                    command.Parameters.AddWithValue("@Price", Product.Price);
                    command.Parameters.AddWithValue("@Quantity", Product.Quantity);
                    command.Parameters.AddWithValue("@Created_At", Product.Created_At);
                    connection.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<ProductDTO> GetProductByID(int ProductID)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetProductByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID", ProductID);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new ProductDTO(
                                 reader.GetInt32(reader.GetOrdinal("ProductID")),
                                 reader.GetInt32(reader.GetOrdinal("CreatedByEmployeeID")),
                                 reader.GetString(reader.GetOrdinal("ProductName")),
                                 reader.GetString(reader.GetOrdinal("ProductDescreption")),
                                 reader.GetDecimal(reader.GetOrdinal("Price")),
                                 reader.GetInt32(reader.GetOrdinal("Quantity")),
                                 reader.GetDateTime(reader.GetOrdinal("Created_At"))
                                 );
                        }
                    }
                }
            }
            return null;
        }
        public async Task<IList<ProductDTO>> GetAllProducts()
        {
            var ProductsList = new List<ProductDTO>();
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetAllProducts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            ProductsList.Add(new ProductDTO(
                                 reader.GetInt32(reader.GetOrdinal("ProductID")),
                                 reader.GetInt32(reader.GetOrdinal("CreatedByEmployeeID")),
                                 reader.GetString(reader.GetOrdinal("ProductName")),
                                 reader.GetString(reader.GetOrdinal("ProductDescreption")),
                                 reader.GetDecimal(reader.GetOrdinal("Price")),
                                 reader.GetInt32(reader.GetOrdinal("Quantity")),
                                 reader.GetDateTime(reader.GetOrdinal("Created_At"))));
                        }
                        return ProductsList;
                    }
                }
            }
            return null;
        }
        public async Task<bool> DeleteProduct(int ProductID)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spDeleteProduct", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID", ProductID);
                    conn.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected == 1;
        }
        public async Task<int>IsProductExist(int ProductID)
        {
            int Found = -1;
            using (SqlConnection connection = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spIsProductExist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID", ProductID);
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
