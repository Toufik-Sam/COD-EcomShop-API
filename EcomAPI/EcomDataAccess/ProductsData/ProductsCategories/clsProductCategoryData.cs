using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.ProductsData.ProductsCategories
{
    public class clsProductCategoryData : IProductCategoryData
    {
        private readonly IDataAccessSettings _settings;

        public clsProductCategoryData(IDataAccessSettings settings)
        {
            this._settings = settings;
        }
        public async Task<int> AddProductCategories(ProductCategoryDTO ProductCategory)
        {
            int NewProductCategoryID = -1;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spAddProductCategories", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID", ProductCategory.ProductID);
                    command.Parameters.AddWithValue("@CategoryID", ProductCategory.CategoryID);

                    var OutputIdParam = new SqlParameter("@NewProductCategoryID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(OutputIdParam);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                    NewProductCategoryID = (int)OutputIdParam.Value;
                }
            }
            return NewProductCategoryID;
        }
        public async Task<bool> UpdateProductCategories(ProductCategoryDTO ProductCategory)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spUpdateProductCategories", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductCategoryID", ProductCategory.ProductCategoryID);
                    command.Parameters.AddWithValue("@ProductID", ProductCategory.ProductID);
                    command.Parameters.AddWithValue("@CategoryID", ProductCategory.CategoryID);
                    connection.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<IList<ProductCategoryDTO>> GetProductCategoriesByID(int ProductID)
        {
            var ProductCategoriesList = new List<ProductCategoryDTO>();
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetProductCategories", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID", ProductID);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            ProductCategoriesList.Add(new ProductCategoryDTO(
                                 reader.GetInt32(reader.GetOrdinal("ProductCategoryID")),
                                 reader.GetInt32(reader.GetOrdinal("ProductID")),
                                 reader.GetInt32(reader.GetOrdinal("CategoryID"))));
                        }
                        return ProductCategoriesList;
                    }
                }
            }
            return null;
        }
        public async Task<ProductCategoryDTO>GetProductCategoryByID(int ProductCategoryID)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetProductCategoryByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductCategoryID", ProductCategoryID);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new ProductCategoryDTO(
                                 reader.GetInt32(reader.GetOrdinal("ProductCategoryID")),
                                 reader.GetInt32(reader.GetOrdinal("ProductID")),
                                 reader.GetInt32(reader.GetOrdinal("CategoryID"))
                                 );
                        }
                    }
                }
            }
            return null;
        }
    }
}
