using System.Data.SqlClient;
using System.Data;
namespace EcomDataAccess.SalesData.SalesManagement.CustomerBehavior
{
    public class clsCustomerBehaviorData : ICustomerBehaviorData
    {
        private readonly IDataAccessSettings _settings;

        public clsCustomerBehaviorData(IDataAccessSettings settings)
        {
            _settings = settings;
        }
        public async Task<IList<TopCustomerDTO>> GetTopCustomers(int Top = -1)
        {
            var TopCustomersByPurchaseHistoryList = new List<TopCustomerDTO>();
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetTopCustomers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Top", Top);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            TopCustomersByPurchaseHistoryList.Add(new TopCustomerDTO(
                                 reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                 reader.GetString(reader.GetOrdinal("FirstName")),
                                 reader.GetString(reader.GetOrdinal("LastName")),
                                 reader.GetDecimal(reader.GetOrdinal("TotalAmountSpent")),
                                 reader.GetInt32(reader.GetOrdinal("TotalOrders")),
                                 reader.GetDateTime(reader.GetOrdinal("LastPurchaseDate"))));
                        }

                        return TopCustomersByPurchaseHistoryList;
                    }
                }
            }
        }
        public async Task<IList<TopCustomerDTO>> GetTopCustomersByRange(DateTime?StartDate, DateTime?EndDate, int Top = -1)
        {
            var TopCustomersByPurchaseHistoryList = new List<TopCustomerDTO>();
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetTopCustomerByRange", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Top", Top);
                    command.Parameters.AddWithValue("@StartDate", StartDate);
                    command.Parameters.AddWithValue("@EndDate", EndDate);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            TopCustomersByPurchaseHistoryList.Add(new TopCustomerDTO(
                                 reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                 reader.GetString(reader.GetOrdinal("FirstName")),
                                 reader.GetString(reader.GetOrdinal("LastName")),
                                 reader.GetDecimal(reader.GetOrdinal("TotalAmountSpent")),
                                 reader.GetInt32(reader.GetOrdinal("TotalOrders")),
                                 reader.GetDateTime(reader.GetOrdinal("LastDatePurchase"))));
                        }

                        return TopCustomersByPurchaseHistoryList;
                    }
                }
            }
        }
        public async Task<CustomerPurchaseHistoryDTO> GetCustomerPurchaseHistory(int CustomerID, DateTime?StartDate, DateTime?EndDate)
        {
            CustomerPurchaseHistoryDTO cutomerPurchaseHistory = new CustomerPurchaseHistoryDTO(-1,"","",new List<CustomerOrderDTO>());
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetCustomerPurchaseHistory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", CustomerID);
                    command.Parameters.AddWithValue("@StartDate", StartDate);
                    command.Parameters.AddWithValue("@EndDate", EndDate);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        bool flag = true;
                        var OrdersList = new List<CustomerOrderDTO>();
                        var OrderProductsList = new List<OrderProductDTO>();
                        CustomerOrderDTO customerOrder = new CustomerOrderDTO(-1,DateTime.Now,0,new List<OrderProductDTO>());
                        OrderProductDTO orderProduct = new OrderProductDTO(-1,"",-1,0);
                        while (reader.Read())
                        {
                            int OrderID = reader.GetInt32(reader.GetOrdinal("OrderID"));
                            if (flag)
                            {
                                cutomerPurchaseHistory.CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID"));
                                cutomerPurchaseHistory.CustomerFirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                                cutomerPurchaseHistory.CustomerLastName = reader.GetString(reader.GetOrdinal("LastName"));
                                customerOrder.OrderID = reader.GetInt32(reader.GetOrdinal("OrderID"));
                                customerOrder.OrderDate = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
                                customerOrder.OrderTotal = reader.GetDecimal(reader.GetOrdinal("TotalAmount"));
                                flag = false;
                            }
                            if (customerOrder.OrderID != OrderID)
                            {
                                customerOrder.Products = OrderProductsList;
                                OrdersList.Add(customerOrder);
                                customerOrder= new CustomerOrderDTO(-1, DateTime.Now, 0, new List<OrderProductDTO>());
                                customerOrder.OrderID = reader.GetInt32(reader.GetOrdinal("OrderID"));
                                customerOrder.OrderDate = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
                                customerOrder.OrderTotal = reader.GetDecimal(reader.GetOrdinal("TotalAmount"));
                                orderProduct = new OrderProductDTO(-1, "", -1, 0);
                                OrderProductsList = new List<OrderProductDTO>();

                            }
                            orderProduct.ProductID = reader.GetInt32(reader.GetOrdinal("ProductID"));
                            orderProduct.ProductName = reader.GetString(reader.GetOrdinal("ProductName"));
                            orderProduct.QuantityPurchased = reader.GetInt32(reader.GetOrdinal("Quantity"));
                            orderProduct.Price = reader.GetDecimal(reader.GetOrdinal("Price"));
                            OrderProductsList.Add(orderProduct);
                            orderProduct = new OrderProductDTO(-1, "", -1, 0);
                        }
                        customerOrder.Products = OrderProductsList;
                        OrdersList.Add(customerOrder);
                        cutomerPurchaseHistory.Orders = OrdersList;
                    }
                }
            }
            return cutomerPurchaseHistory;
        }
    }
}
