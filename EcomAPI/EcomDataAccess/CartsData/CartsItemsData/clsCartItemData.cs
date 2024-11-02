using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.CartsData.CartsItemsData
{
    public class clsCartItemData : ICartItemData
    {
        private readonly IDataAccessSettings _settings;

        public clsCartItemData(IDataAccessSettings settings)
        {
            this._settings = settings;
        }
        public async Task<int> AddNewCartItem(CartItemDTO cartItemDTO)
        {
            int NewCartItemID = -1;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spAddNewCartItem", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CartID", cartItemDTO.CartID);
                    command.Parameters.AddWithValue("@ProductID", cartItemDTO.ProductID);
                    command.Parameters.AddWithValue("@Quantity", cartItemDTO.Quantity);
                    var OutputIdParam = new SqlParameter("@NewCartItemID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(OutputIdParam);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                    NewCartItemID = (int)OutputIdParam.Value;
                }
            }
            return NewCartItemID;
        }
        public async Task<bool> UpdateCartItem(CartItemDTO cartItemDTO)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spUpdateCartItem", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CartItemID", cartItemDTO.CartItemID);
                    command.Parameters.AddWithValue("@CartID", cartItemDTO.CartID);
                    command.Parameters.AddWithValue("@ProductID", cartItemDTO.ProductID);
                    command.Parameters.AddWithValue("@Quantity", cartItemDTO.Quantity);
                    connection.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<CartItemDTO> GetCartItemByID(int CartItemID)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetCartItemByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CartItemID", CartItemID);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new CartItemDTO(
                                 reader.GetInt32(reader.GetOrdinal("CartItemID")),
                                 reader.GetInt32(reader.GetOrdinal("CartID")),
                                 reader.GetInt32(reader.GetOrdinal("ProductID")),
                                 reader.GetInt32(reader.GetOrdinal("Quantity"))
                                 );
                        }
                    }
                }
            }
            return null;
        }
        public async Task<IList<CartItemDTO>> GetAllCardItems(int CartID)
        {
            var CardItemsList = new List<CartItemDTO>();
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spGetAllCartItems", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CartID", CartID);
                    conn.Open();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            CardItemsList.Add(new CartItemDTO(
                                 reader.GetInt32(reader.GetOrdinal("CartItemID")),
                                 reader.GetInt32(reader.GetOrdinal("CartID")),
                                 reader.GetInt32(reader.GetOrdinal("ProductID")),
                                 reader.GetInt32(reader.GetOrdinal("Quantity"))));
                        }
                    }
                }
            }
            return CardItemsList;
        }
        public async Task<bool> DeleteCartItem(int CartItemID)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spDeleteCartItem", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CartItemID", CartItemID);
                    conn.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected == 1;
        }
    }
}
