using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.OrdersData.OrderStatus
{
    public class clsOrderStatusData : IOrderStatusData
    {
        private readonly IDataAccessSettings _settings;

        public clsOrderStatusData(IDataAccessSettings settings)
        {
            this._settings = settings;
        }
        public async Task<int> AddNewOrderStatus(OrderStatusDTO orderStatusDTO)
        {
            int NewOrderStatusID = -1;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spAddNewOrderStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StatusName", orderStatusDTO.StatusName);

                    var OutputIdParam = new SqlParameter("@NewOrderStatusID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(OutputIdParam);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                    NewOrderStatusID = (int)OutputIdParam.Value;
                }
            }
            return NewOrderStatusID;
        }
        public async Task<bool> UpdateOrderStatus(OrderStatusDTO orderStatusDTO)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spUpdateOrderStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderStatusID", orderStatusDTO.OrderStatusID);
                    command.Parameters.AddWithValue("@StatusName", orderStatusDTO.StatusName);
                    connection.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<OrderStatusDTO> GetOrderStatusByID(int OrderStatusID)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetStatusOrderByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderStatusID", OrderStatusID);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new OrderStatusDTO(
                                 reader.GetInt32(reader.GetOrdinal("OrderStatusID")),
                                 reader.GetString(reader.GetOrdinal("StatusName"))
                                 );
                        }
                    }
                }
            }
            return null;
        }
        public async Task<IList<OrderStatusDTO>> GetAllOrderStatuses()
        {
            var OrderStatusList = new List<OrderStatusDTO>();
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spGetAllOrderStatuses", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            OrderStatusList.Add(new OrderStatusDTO(
                                 reader.GetInt32(reader.GetOrdinal("OrderStatusID")),
                                 reader.GetString(reader.GetOrdinal("StatusName"))
                                 ));
                        }
                    }
                }
            }
            return OrderStatusList;
        }
        public async Task<bool> DeleteOrderStatus(int OrderStatusID)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spDeleteOrderStatus", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderStatusID", OrderStatusID);
                    conn.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
    }
}
