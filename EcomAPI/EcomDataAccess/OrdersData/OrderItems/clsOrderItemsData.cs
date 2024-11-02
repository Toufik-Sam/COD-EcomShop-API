using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.OrdersData.OrderItems
{
    public class clsOrderItemsData : IOrderItemsData
    {
        private readonly IDataAccessSettings _settings;

        public clsOrderItemsData(IDataAccessSettings settings)
        {
            this._settings = settings;
        }
        public async Task<int> AddNewOrderItem(OrderItemDTO orderItem)
        {
            int newOrderItemID = -1;
            using (SqlConnection connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spAddNewOrderItem", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID", orderItem.ProductID);
                    command.Parameters.AddWithValue("@OrderID", orderItem.OrderID);
                    command.Parameters.AddWithValue("@Price", orderItem.Price);
                    command.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
                    var OutputIdParam = new SqlParameter("@NewOrderItemID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(OutputIdParam);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                    newOrderItemID = (int)OutputIdParam.Value;
                }
            }
            return newOrderItemID;
        }
        public async Task<bool> UpdateOrderItem(OrderItemDTO orderItem)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spUpdateOrderItem", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderItemID", orderItem.OrderItemID);
                    command.Parameters.AddWithValue("@ProductID", orderItem.ProductID);
                    command.Parameters.AddWithValue("@OrderID", orderItem.OrderID);
                    command.Parameters.AddWithValue("@Price", orderItem.Price);
                    command.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
                    connection.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<OrderItemDTO> GetOrderItemByID(int OrderItemID)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetOrderItemByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderItemID", OrderItemID);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new OrderItemDTO(
                                 reader.GetInt32(reader.GetOrdinal("OrderItemID")),
                                 reader.GetInt32(reader.GetOrdinal("ProductID")),
                                 reader.GetInt32(reader.GetOrdinal("OrderID")),
                                 reader.GetDecimal(reader.GetOrdinal("Price")),
                                 reader.GetInt32(reader.GetOrdinal("Quantity"))
                                 );
                        }
                    }
                }
            }
            return null;
        }
        public async Task<IList<OrderItemDTO>> GetAllOrderItems(int OrderID)
        {
            var OrderItemsList = new List<OrderItemDTO>();
            using (SqlConnection connection = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spGetAllOrderItems", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderID", OrderID);
                    connection.Open();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            OrderItemsList.Add(new OrderItemDTO(
                                 reader.GetInt32(reader.GetOrdinal("OrderItemID")),
                                 reader.GetInt32(reader.GetOrdinal("ProductID")),
                                 reader.GetInt32(reader.GetOrdinal("OrderID")),
                                 reader.GetDecimal(reader.GetOrdinal("Price")),
                                 reader.GetInt32(reader.GetOrdinal("Quantity"))));
                        }
                    }
                }
            }
            return OrderItemsList;
        }
        public async Task<bool> DeleteOrderItem(int OrderItemID)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spDeleteOrderItem", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderItemID", OrderItemID);
                    conn.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected == 1;
        }

    }
}
