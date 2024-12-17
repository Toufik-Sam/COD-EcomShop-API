using EcomBusinessLayer.Sales.SalesDetails;
using EcomBusinessLayer.Sales.SalesDetails.CustomerBehavior;
using EcomBusinessLayer.Sales.SalesDetails.ProductInsight;
using EcomDataAccess.SalesData.SalesManagement;
using EcomDataAccess.SalesData.SalesManagement.CustomerBehavior;
using EcomDataAccess.SalesData.SalesManagement.ProductInsight;
using EcommerceAppAPI.Utility;
using Microsoft.AspNetCore.Mvc;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EcommerceAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesDepartmentController : ControllerBase
    {
        private readonly ISaleManagement _saleManagement;
        private readonly ICustomerBehavior _customerBehavior;
        private readonly IProductInsight _productInsight;
        private readonly ILogger<SalesDepartmentController> _logger;

        public SalesDepartmentController(ISaleManagement saleManagement,ICustomerBehavior customerBehavior,
            IProductInsight productInsight,ILogger<SalesDepartmentController>logger)
        {
            this._saleManagement = saleManagement;
            this._customerBehavior = customerBehavior;
            this._productInsight = productInsight;
            this._logger = logger;
        }

        [HttpGet("SalesByRegion")]
        public async Task<ActionResult<SaleByRegionDTO>> GetSaleByRegion([FromQuery] SalesByRegionArgs Args)
        {
            _logger.LogInformation(message: "GET: api/SalesByRegion");
            if (!clsUtil.ValidateLettersOnly(Args.Country) || !clsUtil.ValidateLettersOnly(Args.City))
                return BadRequest("Invalid Data");
            try
            {
                var SalesByRegion = await _saleManagement.GetSaleByRegion(Args);
                return Ok(SalesByRegion);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, message: "The GET: api/SalesByRegion failed!");
                return BadRequest();
            }
        }

        [HttpGet("DaySalesSummary")]
        public async Task<ActionResult<DailySalesSummaryDTO>>GetDailySaleSummary(DateTime date)
        {
            _logger.LogInformation(message: "GET: api/DaySalesSummary");
            try
            {
                var dailySalesSummary = await _saleManagement.GetDailySaleSummary(date);
                return Ok(dailySalesSummary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET: api/DaySalesSummary failed!");
                return BadRequest();
            }
        }

        [HttpGet("MonthlySalesTrends")]
        public async Task<ActionResult<MonthlySaleTrendDTO>> GetMonthlySalesTrends(int year,int month)
        {
            _logger.LogInformation(message: "GET: api/MonthlySalesTrends");
            if (year < 1 || month < 1 || month > 12)
                return BadRequest("Invalid Data");
            try
            {
                var MonthSalesTrends = await _saleManagement.GetMonthlySalesTrends(year,month);
                return Ok(MonthSalesTrends);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET: api/MonthlySalesTrends failed!");
                return BadRequest();
            }
        }

        [HttpGet("TopCustomers")]
        public async Task<ActionResult<IList<TopCustomerDTO>>>GetTopCustomers(int Top=-1)
        {
            _logger.LogInformation(message: "GET: api/TopCustomers");
            try
            {
                var TopCustomers = await _customerBehavior.GetTopCustomer(Top);
                return Ok(TopCustomers);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, message: "The GET: api/TopCustomers failed!");
                return BadRequest();
            }
        }

        [HttpGet("TopCustomersByRange")]
        public async Task<ActionResult<IList<TopCustomerDTO>>>GetTopCustomersByRange(DateTime?StartDate,DateTime?EndDate,int Top=-1)
        {
            _logger.LogInformation(message: "GET: api/TopCustomersByRange");
            if (StartDate == null || EndDate == null)
                return BadRequest("Inavlid Data!");
            try
            {
                var TopCustomers = await _customerBehavior.GetTopCustomerByRange(StartDate,EndDate,Top);
                return Ok(TopCustomers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET: api/TopCustomersByRange failed!");
                return BadRequest();
            }
        }

        [HttpGet("CustomerPurchaseHistory")]
        public async Task<ActionResult<CustomerPurchaseHistoryDTO>>GetCustomerPurchaseHistory(int CustomerID,DateTime?StartDate,
            DateTime?EndDate)
        {
            _logger.LogInformation(message: "GET: api/CustomerPurchaseHistory");
            if (CustomerID < 1 || StartDate==null || EndDate==null)
                return BadRequest("Invalid Data");
            try
            {
                var CustomerPurchaseHistory = await _customerBehavior.GetCustomerPurchaseHistory(CustomerID,StartDate,EndDate);
                return Ok(CustomerPurchaseHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET: api/CustomerPurchaseHistory failed!");
                return BadRequest();
            }
        }

        [HttpGet("ProductsList")]
        public async Task<ActionResult<IList<SoldProductDTO>>>GetSoldProductsList()
        {
            _logger.LogInformation(message: "GET: api/SoldProductsList");
            try
            {
                var SoldProductsList = await _productInsight.GetSoldProductsList();
                return Ok(SoldProductsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET: api/SoldProductsList failed!");
                return BadRequest();
            }
        }

        [HttpGet("TopSellingProductsByRange")]
        public async Task<ActionResult<IList<SoldProductDTO>>> GetTopSellingProductsListByDateRange(DateTime?StartDate,DateTime?EndDate,
            int Top=-1)
        {
            _logger.LogInformation(message: "GET: api/TopSellingProductsByRange");
            if (StartDate == null || EndDate == null)
                return BadRequest("Invalid Data!");
            try
            {
                var TopSellingProductsByRange = await _productInsight.GetTopSellingProductsListByDateRange(StartDate,EndDate,Top);
                return Ok(TopSellingProductsByRange);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET: api/TopSellingProductsByRange failed!");
                return BadRequest();
            }
        }

    }
}
