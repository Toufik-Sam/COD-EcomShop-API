﻿using EcomBusinessLayer.Orders;
using EcomBusinessLayer.Sales;
using EcomDataAccess;
using EcomDataAccess.SalesData;
using EcommerceAppAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISale _sale;
        private readonly IOrder _order;
        private readonly ILogger<SalesController> _logger;
        private readonly IUserService _global;

        public SalesController(ISale sale,IOrder order,ILogger<SalesController>logger, IUserService global)
        {
            this._sale = sale;
            this._order = order;
            this._logger = logger;
            this._global = global;
        }
        [HttpGet("AllSales")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<SaleDTO>>>GetAllSales()
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
                && (_global.GetUser().Permission & Permissions.SalesManager) != Permissions.SalesManager)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/AllSales");
            try
            {
                var SalesList = await _sale.GetAllSales();
                return Ok(SalesList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/AllSales failed!");
                return BadRequest();
            }
        }

        [HttpGet("AllFullSales")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<FullSaleDTO>>> GetAllFullSales()
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
              && (_global.GetUser().Permission & Permissions.SalesManager) != Permissions.SalesManager)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/AllSales");
            try
            {
                var FullSalesList = await _sale.GetAllFullSales();
                return Ok(FullSalesList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/AllFullSales failed!");
                return BadRequest();
            }
        }

        [HttpGet("{id}/SaleByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SaleDTO>>GetSaleByID(int id)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
             && (_global.GetUser().Permission & Permissions.SalesManager) != Permissions.SalesManager)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/{id}/SaleByID", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                SaleDTO sale = await _sale.Find(id);
                if (sale == null)
                    return NotFound($"sale With ID {id} was not found");
                return Ok(sale);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/{id}/SaleByID failed!", id);
                return BadRequest();
            }
        }

        [HttpGet("{id}/FullSaleByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SaleDTO>> GetFullSaleByID(int id)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
              && (_global.GetUser().Permission & Permissions.SalesManager) != Permissions.SalesManager)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/{id}/FullSaleByID", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                FullSaleDTO fullSale = await _sale.FindFull(id);
                if (fullSale == null)
                    return NotFound($"sale With ID {id} was not found");
                return Ok(fullSale);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/{id}/FullSaleByID failed!", id);
                return BadRequest();
            }
        }

        [HttpPost("AddNewSale")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SaleDTO>>AddNewSale(SaleDTO newSale)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
                && (_global.GetUser().Permission & Permissions.SalesManager) != Permissions.SalesManager)
                return Unauthorized();
            _logger.LogInformation(message: "POST: api/AddNewSale");
            bool flag = await _order.IsOrderExist(newSale.OrderID);
            if (newSale.Amount<=0 || !flag)
                return BadRequest("Invalid Data!");
            try
            {
                var AddedSale = await _sale.AddNewSale(newSale);
                if (AddedSale != null)
                    return Ok(AddedSale);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(message: "The POST call to api/AddNewSale failed!");
                return BadRequest();
            }
        }

        [HttpPut("{id}/UpdateSale")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult>UpdateSale(int id, SaleDTO UpdatedSale)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
               && (_global.GetUser().Permission & Permissions.SalesManager) != Permissions.SalesManager)
                return Unauthorized();
            _logger.LogInformation(message: "POST: api/UpdateSale");
            bool flag = await _order.IsOrderExist(UpdatedSale.OrderID);
            if (id < 1 || UpdatedSale.Amount<=0 || !flag)
                return BadRequest("Invalid Data");
            try
            {
                var sale = await _sale.Find(id);
                if (sale == null)
                    return NotFound($"Sale With ID {id} was not found");
                sale.OrderID = UpdatedSale.OrderID;
                sale.Amount = UpdatedSale.Amount;
                flag = await _sale.UpdateSale(sale);
                if (flag)
                    return Ok(sale);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/{id}/UpdateSale failed", id);
                return BadRequest();
            }
        }

        [HttpDelete("{id}/DeleteSale")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteSale(int id)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
              && (_global.GetUser().Permission & Permissions.SalesManager) != Permissions.SalesManager)
                return Unauthorized();
            _logger.LogInformation("DELETE: api/{id}/DeleteSale", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                bool flag = await _sale.DeleteSale(id);
                if (flag)
                    return Ok($"Sale with ID {id} was Deleted Successfully!");
                else
                    return BadRequest();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "the DELETE call to api/{id}/DeleteSale failed", id);
                return BadRequest();
            }
        }
    }
}
