using System.Data;
using System.Data.SqlClient;
namespace EcomDataAccess.SalesData.SalesManagement
{
    public class clsSalesManagementData : ISalesManagementData
    {
        private readonly IDataAccessSettings _settings;
        public clsSalesManagementData(IDataAccessSettings settings)
        {
            this._settings = settings;
        }
        public async Task<SaleByRegionDTO> GetSaleByRegion(SalesByRegionArgs Args)
        {
            SaleByRegionDTO SaleByRegion =new SaleByRegionDTO("","",0,0,0,DateTime.Now,DateTime.Now,new List<SaleProductDTO>());
            var ProductList = new List<SaleProductDTO>();
            using (SqlConnection connection = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spGetSaleByRegion", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Country", Args.Country);
                    command.Parameters.AddWithValue("@City", Args.City);
                    command.Parameters.AddWithValue("@StartDate", Args.StartDate);
                    command.Parameters.AddWithValue("@EndDate", Args.EndDate);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        bool flag = true;
                        while (reader.Read())
                        {
                            if (flag)
                            {
                                SaleByRegion.Country = reader.GetString(reader.GetOrdinal("Country"));
                                SaleByRegion.City = reader.GetString(reader.GetOrdinal("City"));
                                SaleByRegion.TotalSales = reader.GetDecimal(reader.GetOrdinal("TotalSales"));
                                SaleByRegion.TotalQuantitySold = reader.GetInt32(reader.GetOrdinal("TotalQuantitySold"));
                                SaleByRegion.NumberOfShippedOrders = reader.GetInt32(reader.GetOrdinal("NumberOfOrders"));
                                SaleByRegion.FirstDatePurchase = reader.GetDateTime(reader.GetOrdinal("FirstDateSale"));
                                SaleByRegion.LastDatePurchase = reader.GetDateTime(reader.GetOrdinal("LastDateSale"));
                                flag = false;
                            }
                            ProductList.Add(new SaleProductDTO(
                                reader.GetInt32(reader.GetOrdinal("ProductID")),
                                reader.GetString(reader.GetOrdinal("ProductName")),
                                reader.GetDecimal(reader.GetOrdinal("TotalRevenue")),
                                reader.GetDecimal(reader.GetOrdinal("Price")),
                                reader.GetInt32(reader.GetOrdinal("QuantitySold"))
                                ));
                        }
                        SaleByRegion.ProductsList = ProductList;
                    }
                }
            }
            return SaleByRegion;
        }
        public async Task<DailySalesSummaryDTO> GetDailySaleSummary(DateTime Date)
        {
            DailySalesSummaryDTO DailySalesSummary = new DailySalesSummaryDTO(DateTime.Now,0,0,0,new List<SaleProductDTO>());
            var ProductList = new List<SaleProductDTO>();
            using (SqlConnection connection = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spGetDailySalesSummary", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Date", Date);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        bool flag = true;
                        while (reader.Read())
                        {
                            if (flag)
                            {
                                DailySalesSummary.Date = reader.GetDateTime(reader.GetOrdinal("Date"));
                                DailySalesSummary.TotalRevenue = reader.GetDecimal(reader.GetOrdinal("TotalRevenue"));
                                DailySalesSummary.TotalQuantitySold = reader.GetInt32(reader.GetOrdinal("TotalQuantitySold"));
                                DailySalesSummary.NumberOfShippedOrders = reader.GetInt32(reader.GetOrdinal("NumberOfShippedOrders"));
                                flag = false;
                            }
                            ProductList.Add(new SaleProductDTO(
                                reader.GetInt32(reader.GetOrdinal("ProductID")),
                                reader.GetString(reader.GetOrdinal("ProductName")),
                                reader.GetDecimal(reader.GetOrdinal("TotalProductRevenue")),
                                reader.GetDecimal(reader.GetOrdinal("Price")),
                                reader.GetInt32(reader.GetOrdinal("QuantitySold"))
                                ));
                        }
                        DailySalesSummary.ProductsList = ProductList;
                    }
                }
            }
            return DailySalesSummary;
        }
        public async Task<MonthlySaleTrendDTO> GetMonthlySalesTrends(int year, int month)
        {
            MonthlySaleTrendDTO MonthSalesTrends = new MonthlySaleTrendDTO(0,0,0,0,DateTime.Now,0,new List<DailySalesSummaryDTO>());
            short Day = 0;
            if (month == 4 || month == 6 || month == 9 || month == 11)
                Day = 30;
            else if (month == 1 || month == 3 || month == 7 || month == 8 || month == 10 || month == 12)
                Day = 31;
            else if (DateTime.IsLeapYear(year))
                Day = 29;
            else
                Day = 28;

            using (SqlConnection connection = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spGetMonthlySalesTrends", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    DateTime FirstDayOfMonth = new DateTime(year, month, 1);
                    DateTime LastDayOfMonth = new DateTime(year,month,Day);
                    command.Parameters.AddWithValue("@StartDate", FirstDayOfMonth);
                    command.Parameters.AddWithValue("@EndDate", LastDayOfMonth);
                    connection.Open();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        bool MonthResume = true;
                        bool DailySalesDetails = true;
                        var ProductsList = new List<SaleProductDTO>();
                        DailySalesSummaryDTO DailySales = new DailySalesSummaryDTO(DateTime.Now, 0, 0, 0, new List<SaleProductDTO>());
                        while (reader.Read())
                        {
                            DateTime CurrentDate = reader.GetDateTime(reader.GetOrdinal("Date"));
                            if (MonthResume)
                            {
                                DailySales.Date = reader.GetDateTime(reader.GetOrdinal("Date"));
                                MonthSalesTrends.Year = year;
                                MonthSalesTrends.Month = month;
                                MonthSalesTrends.TotalMonthlyRevenue = reader.GetDecimal(reader.GetOrdinal("TotalMonthlyRevenue"));
                                MonthSalesTrends.TotalQuantitySold = reader.GetInt32(reader.GetOrdinal("TotalQuantitySold"));
                                MonthSalesTrends.TotalTransactions = reader.GetInt32(reader.GetOrdinal("NumberOfShippedOrders"));
                                MonthSalesTrends.BestSellingDay = reader.GetDateTime(reader.GetOrdinal("BestSellingDay"));
                                MonthSalesTrends.BestSellingDayRevenue = reader.GetDecimal(reader.GetOrdinal("BestSellingDayRevenue"));
                                MonthResume = false;
                            }
                            if (DailySales.Date != CurrentDate)
                            {
                                DailySales.ProductsList = ProductsList;
                                MonthSalesTrends.DailySales.Add(DailySales);
                                ProductsList = new List<SaleProductDTO>();
                                DailySales= new DailySalesSummaryDTO(DateTime.Now, 0, 0, 0, new List<SaleProductDTO>());
                                DailySalesDetails = true;
                            }
                            if (DailySalesDetails)
                            {
                                DailySales.Date = reader.GetDateTime(reader.GetOrdinal("Date"));
                                DailySales.TotalRevenue = reader.GetDecimal(reader.GetOrdinal("TotalRevenue"));
                                DailySales.TotalQuantitySold = reader.GetInt32(reader.GetOrdinal("DayTotalQuantitySold"));
                                DailySales.NumberOfShippedOrders = reader.GetInt32(reader.GetOrdinal("DayNumberOfShippedOrders"));
                                DailySalesDetails = false;
                            }
                            ProductsList.Add(new SaleProductDTO(
                                reader.GetInt32(reader.GetOrdinal("ProductID")),
                                reader.GetString(reader.GetOrdinal("ProductName")),
                                reader.GetDecimal(reader.GetOrdinal("ProductTotalRevenue")),
                                reader.GetDecimal(reader.GetOrdinal("Price")),
                                reader.GetInt32(reader.GetOrdinal("ProductQuantitySold"))
                                ));
                        }
                        DailySales.ProductsList = ProductsList;
                        MonthSalesTrends.DailySales.Add(DailySales);
                    }
                }
            }
            return MonthSalesTrends;
        }
    }
}
