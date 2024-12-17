using System.Data.SqlClient;
using System.Data;

namespace EcomDataAccess.SalesData.SalesManagement.ProductInsight
{
    public class clsProductInsightData : IProductInsightData
    {
        private readonly IDataAccessSettings _settings;

        public clsProductInsightData(IDataAccessSettings settings)
        {
            _settings = settings;
        }
        public async Task<IList<SoldProductDTO>> GetSoldProductsList()
        {
            var SoldProductsList = new List<SoldProductDTO>();
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetSoldProductsList", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            SoldProductsList.Add(new SoldProductDTO(
                                 reader.GetInt32(reader.GetOrdinal("ProductID")),
                                 reader.GetString(reader.GetOrdinal("ProductName")),
                                 reader.GetInt32(reader.GetOrdinal("TotalQuantitySold")),
                                 reader.GetDecimal(reader.GetOrdinal("TotalRevenue")),
                                 reader.GetDecimal(reader.GetOrdinal("Price")),
                                 reader.GetDateTime(reader.GetOrdinal("LastSoldDate"))));
                        }
                        return SoldProductsList;
                    }
                }
            }
        }
        public async Task<IList<SoldProductDTO>> GetTopSellingProductsListByDateRange(DateTime?StartDate, DateTime?EndDate, int Top = -1)
        {
            var SoldProductsListByRange = new List<SoldProductDTO>();
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetTopSellingProductsListByRange", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StartDate", StartDate);
                    command.Parameters.AddWithValue("@EndDate", EndDate);
                    command.Parameters.AddWithValue("@Top", Top);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            SoldProductsListByRange.Add(new SoldProductDTO(
                                 reader.GetInt32(reader.GetOrdinal("ProductID")),
                                 reader.GetString(reader.GetOrdinal("ProductName")),
                                 reader.GetInt32(reader.GetOrdinal("TotalQuantitySold")),
                                 reader.GetDecimal(reader.GetOrdinal("TotalRevenue")),
                                 reader.GetDecimal(reader.GetOrdinal("Price")),
                                 reader.GetDateTime(reader.GetOrdinal("LastSoldDate"))));
                        }
                        return SoldProductsListByRange;
                    }
                }
            }
        }
    }
}
