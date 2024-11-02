using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.CartsData
{
    public class clsCartData : ICartData
    {
        private readonly IDataAccessSettings _settings;
        public clsCartData(IDataAccessSettings settings)
        {
            this._settings = settings;
        }
        public async Task<int> AddNewCart(CartDTO cartDTO)
        {
            int NewCartID = -1;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spAddNewCart", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", cartDTO.CustomerID);
                    var OutputIdParam = new SqlParameter("@NewCartID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(OutputIdParam);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                    NewCartID = (int)OutputIdParam.Value;
                }
            }
            return NewCartID;
        }
        public async Task<bool> UpdateCrat(CartDTO cartDTO)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spUpdateCart", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CartID", cartDTO.CartID);
                    command.Parameters.AddWithValue("@CustomerID", cartDTO.CustomerID);
                    connection.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<CartDTO> GetCartByID(int CartID)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetCartByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CartID", CartID);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new CartDTO(
                                 reader.GetInt32(reader.GetOrdinal("CartID")),
                                 reader.GetInt32(reader.GetOrdinal("CustomerID"))
                                 );
                        }
                    }
                }
            }
            return null;
        }
        public async Task<IList<CartDTO>> GetAllCarts()
        {
            var CardsList = new List<CartDTO>();
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetAllCarts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            CardsList.Add(new CartDTO(
                                 reader.GetInt32(reader.GetOrdinal("CartID")),
                                 reader.GetInt32(reader.GetOrdinal("CustomerID"))));
                        }
                        return CardsList;
                    }
                }
            }
            return null;
        }
        public async Task<bool> DeleteCart(int CartID)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spDeleteCart", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CartID", CartID);
                    conn.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected == 1;
        }
        public async Task<int> IsCartExist(int CardID)
        {
            int Found = -1;
            using (SqlConnection connection = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spIsCartExist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CartID", CardID);
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
