using System.Data.SqlClient;
using System.Data;

namespace EcomDataAccess.OrdersData
{
    public class clsOrdersData : IOrdersData
    {
        private readonly IDataAccessSettings _settings;

        public clsOrdersData(IDataAccessSettings settings)
        {
            this._settings = settings;
        }
        public async Task<int> AddNewOrder(OrderDTO orderDTO)
        {
            int NewOrderID = -1;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spAddNewOrder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", orderDTO.CustomerID);
                    command.Parameters.AddWithValue("@OrderStatusID", orderDTO.OrderStatusID);
                    command.Parameters.AddWithValue("@CreatedAt", orderDTO.CreatedAt);
                    var OutputIdParam = new SqlParameter("@NewOrderID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(OutputIdParam);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                    NewOrderID = (int)OutputIdParam.Value;
                }
            }
            return NewOrderID;
        }
        public async Task<bool>ShipOrder(int OrderID)
        {
            int rowsAffected = 0;
            using(SqlConnection conn=new SqlConnection(_settings.ConnectionString))
            {
                using(SqlCommand command=new SqlCommand("dbo.spShipOrder",conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderID", OrderID);
                    conn.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<bool>DeliverOrder(int OrderID)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spDeliverOrder", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderID", OrderID);
                    conn.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<bool>ConfirmOrder(int OrderID)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spConfirmOrder", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderID", OrderID);
                    conn.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<bool>CancelOrder(int OrderID)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spCancelOrder", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderID", OrderID);
                    conn.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<bool>ReturnOrder(int OrderID)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spReturnOrder", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderID", OrderID);
                    conn.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<OrderDTO> GetOrderByID(int OrderID)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetOrderByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderID", OrderID);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new OrderDTO(
                                 reader.GetInt32(reader.GetOrdinal("OrderID")),
                                 reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                 reader.GetInt32(reader.GetOrdinal("OrderStatusID")),
                                 reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                                 );
                        }
                    }
                }
            }
            return null;
        }
        public async Task<IList<OrderDTO>> GetAllCustomerOrders(int CustomerID)
        {
            var CustomerOrdersList = new List<OrderDTO>();
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spGetAllCustomerOrders", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", CustomerID);
                    conn.Open();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            CustomerOrdersList.Add(new OrderDTO(
                                 reader.GetInt32(reader.GetOrdinal("OrderID")),
                                 reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                 reader.GetInt32(reader.GetOrdinal("OrderStatusID")),
                                 reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                                 ));
                        }
                    }
                }
            }
            return CustomerOrdersList;
        }
        public async Task<bool> DeleteOrderByID(int OrderID)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spDeleteOrder", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderID", OrderID);
                    conn.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected == 1;
        }
        public async Task<int> IsOrderExist(int OrderID)
        {
            int Found = -1;
            using (SqlConnection connection = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spIsOrderExist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderID", OrderID);
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
