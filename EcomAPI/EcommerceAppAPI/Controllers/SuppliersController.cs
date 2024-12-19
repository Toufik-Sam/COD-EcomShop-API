using EcomBusinessLayer.Suppliers;
using EcomBusinessLayer.Suppliers.ProductSuppliers;
using EcomDataAccess.SupplierData;
using EcomDataAccess.SupplierData.ProductSupplierData;
using EcommerceAppAPI.Utility;
using Microsoft.AspNetCore.Mvc;


namespace EcommerceAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplier _supplier;
        private readonly IProductSupplier _productSupplier;
        private readonly ILogger<SuppliersController> _logger;

        public SuppliersController(ISupplier supplier,IProductSupplier productSupplier,ILogger<SuppliersController> logger)
        {
            this._supplier = supplier;
            this._productSupplier = productSupplier;
            this._logger = logger;
        }
        // GET: api/<SuppliersController>
        [HttpGet("AllSuplliers")]
        public async Task<ActionResult<IList<SupplierDTO>>>GetAllSupplier()
        {
            _logger.LogInformation(message: "GET:api/AllSuppliers");
            try
            {
                var supplierList = await _supplier.GetAllSuppliers();
                return Ok(supplierList);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, message: "The GET:api/AllSuppliers failled!");
                return BadRequest(ex);
            }
        }

        [HttpGet("AllProductSuplliers")]
        public async Task<ActionResult<IList<ProductSupplierDTO>>> GetAllProductSupplier(int ProductID)
        {
            _logger.LogInformation(message: "GET:api/AllProductSuplliers");
            if (ProductID < 1)
                return BadRequest("Invalid Data!");
            try
            {
                var ProductSupplierList = await _productSupplier.FindProductSuppliers(ProductID);
                return Ok(ProductSupplierList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET:api/AllProductSuppliers failled!");
                return BadRequest(ex);
            }
        }

        // GET api/<SuppliersController>/5
        [HttpGet("{id}/SupplierBytID")]
        public async Task<ActionResult<SupplierDTO>>GetSupplierByID(int id)
        {
            _logger.LogInformation(message: "GET: api/{id}/SupplierByID", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                SupplierDTO supplier = await _supplier.Find(id);
                if (supplier == null)
                    return NotFound($"Supplier With ID {id} was not found");
                return Ok(supplier);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, message: $"The GET call to api/{id}/SupplierByID Failed");
                return BadRequest(ex);
            }
        }

        // POST api/<SuppliersController>
        [HttpPost("AddNewSupplier")]
        public async Task<ActionResult<SupplierDTO>> AddNewSupplier([FromBody] SupplierDTO newsupplierDTO)
        {
            _logger.LogInformation(message: "POST api/AddNewSupplier");
            if (!ValidateInput(newsupplierDTO))
                return BadRequest("Invalid Data !");
            try
            {
                var AddedSupplier = await _supplier.AddNewSupplier(newsupplierDTO);
                if (AddedSupplier != null)
                    return Ok(AddedSupplier);
                else
                    return BadRequest();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, message: "The Post call to api/AddNewSupplier failed!");
                return BadRequest();
            }
        }

        [HttpPost("AddNewProductSupplier")]
        public async Task<ActionResult<ProductSupplierDTO>> AddNewProductSupplier([FromBody] ProductSupplierDTO newproductsupplierDTO)
        {
            _logger.LogInformation(message: "POST api/AddNewProductSupplier");
            if (newproductsupplierDTO.ProductID<1 || newproductsupplierDTO.SupplierID<1)
                return BadRequest("Invalid Data !");
            try
            {
                var AddedProductSupplier = await _productSupplier.AddNewProductSupplier(newproductsupplierDTO);
                if (AddedProductSupplier != null)
                    return Ok(AddedProductSupplier);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The Post call to api/AddNewProductSupplier failed!");
                return BadRequest();
            }
        }

        // PUT api/<SuppliersController>/5
        [HttpPut("{id}/UpdateSupplier")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] SupplierDTO supplierDTO)
        {
            _logger.LogInformation(message: "POST: api/UpdateSupplier");
            bool flag = await _supplier.IsSupplierExist(id);
            if (id < 1 || ValidateInput(supplierDTO))
                return BadRequest("Invalid Data !");
            try
            {
                var supplier = await _supplier.Find(id);
                if (supplier == null)
                    return NotFound($"Supplier with ID {id} was not found");
                supplier.SupplierName = supplierDTO.SupplierName;
                supplier.Address = supplierDTO.Address;
                supplier.Email = supplierDTO.Email;
                flag = await _supplier.UpdateSupplier(supplierDTO);
                if (flag)
                    return Ok(supplier);
                else
                    return BadRequest();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"The PUT Call to api/{id}/UpdateSupplier failed");
                return BadRequest();
            }
        }

        [HttpPut("{id}/UpdateProductSupplier")]
        public async Task<IActionResult> UpdateProductSupplier(int id, [FromBody] ProductSupplierDTO productSupplierDTO)
        {
            _logger.LogInformation(message: $"PUT: api/{id}/UpdateProductSupplier");
            bool flag = await _productSupplier.IsProductSupplierExist(id);
            if (id < 1 || productSupplierDTO.ProductID<1 || productSupplierDTO.SupplierID<1)
                return BadRequest("Invalid Data !");
            try
            {
                var ProductSupplier = await _productSupplier.GetProductSupplierByID(id);
                if (ProductSupplier == null)
                    return NotFound($" record with ID {id} was not found");
                ProductSupplier.ProductID = productSupplierDTO.ProductID;
                ProductSupplier.SupplierID = productSupplierDTO.SupplierID;
                  flag = await _productSupplier.UpdateProductSupplier(ProductSupplier);
                if (flag)
                    return Ok(ProductSupplier);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"The PUT Call to api/{id}/UpdateProductSupplier failed");
                return BadRequest();
            }
        }

        // DELETE api/<SuppliersController>/5
        [HttpDelete("{id}/DeleteSupplier")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE: api/{id}/DeleteSupplier", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                bool flag = await _supplier.DeleteSupplier(id);
                if (flag)
                    return Ok($"Supplier with ID {id} was Deleted Successfully!");
                else
                    return BadRequest();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"The DELETE call api/{id}/DeleteSupplier failed");
                return BadRequest();
            }
        }

        [HttpDelete("{id}/DeleteProductSupplier")]
        public async Task<IActionResult> DeleteProductSupplier(int id)
        {
            _logger.LogInformation("DELETE: api/{id}/DeleteProductSupplier", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                bool flag = await _productSupplier.DeleteProductSupplier(id);
                if (flag)
                    return Ok($"ProductSupplier with ID {id} was Deleted Successfully!");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"The DELETE call api/{id}/DeleteProductSupplier failed");
                return BadRequest();
            }
        }

        private bool ValidateInput(SupplierDTO supplier)
        {
            if (!clsUtil.ValidateEmail(supplier.Email) || !clsUtil.ValidateLettersOnly(supplier.SupplierName)
                || !clsUtil.ValidatePhoneNumber(supplier.Phone) || string.IsNullOrEmpty(supplier.Address))
                return false;
            return true;
        }
    }
}
