using System.Data.SqlClient;
using System.Data;
namespace EcomDataAccess.SalesData
{
    public class clsSaleData : ISaleData
    {
        private readonly IDataAccessSettings _settings;
        public clsSaleData(IDataAccessSettings settings)
        {
            this._settings = settings;
        }
        public async Task<int> AddNewSale(SaleDTO Sale)
        {
            int NewSale = -1;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spAddNewSale", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderID", Sale.OrderID);
                    command.Parameters.AddWithValue("@Amount", Sale.Amount);
                    var OutputIdParam = new SqlParameter("@NewSale", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(OutputIdParam);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                    NewSale = (int)OutputIdParam.Value;
                }
            }
            return NewSale;
        }
        public async Task<bool> UpdateSale(SaleDTO Sale)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spUpdateSale", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SaleID", Sale.SaleID);
                    command.Parameters.AddWithValue("@OrderID", Sale.OrderID);
                    command.Parameters.AddWithValue("@Amount", Sale.Amount);
                    connection.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<SaleDTO> GetSaleByID(int SaleID)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetSaleByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SaleID", SaleID);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new SaleDTO(
                                 reader.GetInt32(reader.GetOrdinal("SaleID")),
                                 reader.GetInt32(reader.GetOrdinal("OrderID")),
                                 reader.GetDecimal(reader.GetOrdinal("Amount"))
                                 );
                        }
                    }
                }
            }
            return null;
        }
        public async Task<FullSaleDTO>GetFullSaleByID(int SaleID)
        {
            FullSaleDTO fullSaleDTO=null;
            var OrderItemsList = new List<OrderItemInfoDTO>();
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetFullSaleByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SaleID", SaleID);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        bool flag = true;
                        while (reader.Read())
                        {
                            int ID = reader.GetInt32(reader.GetOrdinal("SaleID"));
                            if(flag)
                            {
                                fullSaleDTO.SaleID = reader.GetInt32(reader.GetOrdinal("SaleID"));
                                fullSaleDTO.OrderID = reader.GetInt32(reader.GetOrdinal("OrderID"));
                                fullSaleDTO.Amount = reader.GetDecimal(reader.GetOrdinal("Amount"));
                                flag = false;
                            }
                            OrderItemsList.Add(new OrderItemInfoDTO(
                                reader.GetInt32(reader.GetOrdinal("OrderItemID")),
                                reader.GetInt32(reader.GetOrdinal("ProductID")),
                                reader.GetString(reader.GetString("ProductName")),
                                reader.GetDecimal(reader.GetOrdinal("Price")),
                                reader.GetInt32(reader.GetOrdinal("Quantity"))
                                ));
                        }
                        fullSaleDTO.OrderItems = OrderItemsList;
                    }
                }
            }
            return fullSaleDTO;
        }
        public async Task<IList<SaleDTO>> GetAllSales()
        {
            var SalesList = new List<SaleDTO>();
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetAllSales", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            SalesList.Add(new SaleDTO(
                                 reader.GetInt32(reader.GetOrdinal("SaleID")),
                                 reader.GetInt32(reader.GetOrdinal("OrderID")),
                                 reader.GetDecimal(reader.GetOrdinal("Amount"))));
                        }
                        return SalesList;
                    }
                }
            }
        }
        public async Task<IList<FullSaleDTO>>GetAllFullSales()
        {
            var FullSalesList = new List<FullSaleDTO>();
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetAllFullSales", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        FullSaleDTO fullSaleDTO = null;
                        var ProductsList = new List<OrderItemInfoDTO>();
                        bool flag = true;
                        while (reader.Read())
                        {
                            int SaleID = reader.GetInt32(reader.GetOrdinal("SaleID"));
                           if(flag)
                           {
                                fullSaleDTO.SaleID = reader.GetInt32(reader.GetOrdinal("SaleID"));
                                fullSaleDTO.OrderID = reader.GetInt32(reader.GetOrdinal("OrderID"));
                                fullSaleDTO.Amount = reader.GetDecimal(reader.GetOrdinal("Amount"));
                                flag = false;
                           }
                           if(SaleID!=fullSaleDTO.SaleID)
                           {
                                fullSaleDTO.OrderItems = ProductsList;
                                FullSalesList.Add(fullSaleDTO);
                                ProductsList = null;
                                fullSaleDTO.SaleID = reader.GetInt32(reader.GetOrdinal("SaleID"));
                                fullSaleDTO.OrderID = reader.GetInt32(reader.GetOrdinal("OrderID"));
                                fullSaleDTO.Amount = reader.GetDecimal(reader.GetOrdinal("Amount"));
                            }
                            ProductsList.Add(new OrderItemInfoDTO(
                                reader.GetInt32(reader.GetOrdinal("OrderItemID")),
                                reader.GetInt32(reader.GetOrdinal("ProductID")),
                                reader.GetString(reader.GetString("ProductName")),
                                reader.GetDecimal(reader.GetOrdinal("Price")),
                                reader.GetInt32(reader.GetOrdinal("Quantity"))
                                ));
                        }
                        fullSaleDTO.OrderItems = ProductsList;
                        FullSalesList.Add(fullSaleDTO);
                    }
                }
            }
            return FullSalesList;
        }
        public async Task<bool> DeleteSale(int SaleID)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spDeleteSale", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SaleID", SaleID);
                    conn.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected == 1;
        }
        public async Task<int> IsSaleExit(int SaleID)
        {
            int Found = -1;
            using (SqlConnection connection = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spIsSaleExit", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SaleID", SaleID);
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
