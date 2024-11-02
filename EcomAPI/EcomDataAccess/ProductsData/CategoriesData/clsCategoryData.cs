using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.ProductsData.CategoriesData
{
    public class clsCategoryData : ICategoryData
    {
        private readonly IDataAccessSettings _settings;
        public clsCategoryData(IDataAccessSettings settings)
        {
            this._settings = settings;
        }
        public async Task<int> AddCategory(CategoryDTO category)
        {
            int NewCategoryID = -1;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spAddNewCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeID", category.EmployeeID);
                    command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    command.Parameters.AddWithValue("@CategoryDescription", category.CategoryDescription);
                    var OutputIdParam = new SqlParameter("@NewCategoryID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(OutputIdParam);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                    NewCategoryID = (int)OutputIdParam.Value;
                }
            }
            return NewCategoryID;
        }
        public async Task<bool> UpdateCategory(CategoryDTO category)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spUpdateCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CategoryID", category.CategoryID);
                    command.Parameters.AddWithValue("@EmployeeID", category.EmployeeID);
                    command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    command.Parameters.AddWithValue("@CategoryDescription", category.CategoryDescription);
                    connection.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<CategoryDTO> GetCategoryByID(int CategoryID)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetCategoryByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CategoryID", CategoryID);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new CategoryDTO(
                                 reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                 reader.GetInt32(reader.GetOrdinal("CreatedByEmployeeID")),
                                 reader.GetString(reader.GetOrdinal("CategoryName")),
                                 reader.GetString(reader.GetOrdinal("CategoryDescription"))
                                 );
                        }
                    }
                }
            }
            return null;
        }
        public async Task<IList<CategoryDTO>> GetAllCategories()
        {
            var ProductsList = new List<CategoryDTO>();
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetAllCategories", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            ProductsList.Add(new CategoryDTO(
                                 reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                 reader.GetInt32(reader.GetOrdinal("CreatedByEmployeeID")),
                                 reader.GetString(reader.GetOrdinal("CategoryName")),
                                 reader.GetString(reader.GetOrdinal("CategoryDescription"))));
                        }
                        return ProductsList;
                    }
                }
            }
            return null;
        }
        public async Task<bool> DeleteCategory(int CategoryID)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spDeleteCategory", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CategoryID", CategoryID);
                    conn.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected == 1;
        }
        public async Task<int>IsCategoryExist(int categoryID)
        {
            int Found = -1;
            using (SqlConnection connection = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spIsCategoryExist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@categoryID", categoryID);
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
