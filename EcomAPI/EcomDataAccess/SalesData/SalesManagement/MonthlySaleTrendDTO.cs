namespace EcomDataAccess.SalesData.SalesManagement
{
    public class MonthlySaleTrendDTO
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalMonthlyRevenue { get; set; }
        public int TotalQuantitySold { get; set; }
        public int TotalTransactions { get; set; }
        public DateTime BestSellingDay { get; set; }
        public decimal BestSellingDayRevenue { get; set; }
        public IList<DailySalesSummaryDTO> DailySales { get; set; }
        public MonthlySaleTrendDTO(int Year,int Month,decimal TotalMonthlyRevenue,int TotalQuantitySold,
            DateTime BestSellingDay,decimal BestSellingDayRevenue,IList<DailySalesSummaryDTO>DailySales)
        {
            this.Year = Year;
            this.Month = Month;
            this.TotalMonthlyRevenue = TotalMonthlyRevenue;
            this.TotalQuantitySold = TotalQuantitySold;
            this.BestSellingDay = BestSellingDay;
            this.BestSellingDayRevenue = BestSellingDayRevenue;
            this.DailySales = DailySales;
        }
    }
}
