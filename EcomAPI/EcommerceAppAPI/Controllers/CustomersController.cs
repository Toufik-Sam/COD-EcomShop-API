using EcomBusinessLayer.Customers;
using EcomDataAccess;
using EcomDataAccess.CustomersData;
using EcomDataAccess.CustomersData.CustomersAddresses;
using EcommerceAppAPI.Models;
using EcommerceAppAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EcommerceAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomer _customer;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomer customer,ILogger<CustomersController>logger)
        {
            this._customer = customer;
            this._logger = logger;
        }
        // GET: api/<CustomersController>
        [HttpGet("AllCustomers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<CustomerDTO>>> GetAllCustomers()
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/AllCustomers");
            try
            {
                var CustomersList = await _customer.GetAllCustomers();
                return Ok(CustomersList);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/AllCustomers failed!");
                return BadRequest();
            }
        }

        // GET api/<CustomersController>/5
        [HttpGet("{id}/CustomerByID",Name ="GetCustomerByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerDTO>> GetCutomerByID(int id)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin &&
               (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation(message: "GET: api/{id}/CustomerByID", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                CustomerDTO customer = await _customer.Find(id);
                if (customer == null)
                    return NotFound($"Customer With ID {id} was not found");
                return Ok(customer);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/{id}/CustomerByID failed!", id);
                return BadRequest();
            }
        }

        [HttpGet("{id}/CustomerAddressID", Name = "GetCustomerAddressByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerAddressDTO>> GetCutomerAddressByID(int id)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation(message: "GET: api/{id}/CustomerAddressID", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                CustomerAddressDTO customerAddress = await _customer.GetCustomerAddressByID(id);
                if (customerAddress == null)
                    return NotFound($"CustomerAddressID With ID {id} was not found");
                return Ok(customerAddress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/{id}/CustomerAddressID failed!", id);
                return BadRequest();
            }
        }

        [HttpGet("{id}/AllCustomerAddresses", Name = "GetAllCustomerAddresses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<CustomerAddressDTO>>> GetCutomerAddresses(int id)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin)
                return Unauthorized();

            _logger.LogInformation(message: "GET: api/{id}/AllCustomerAddresses", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                var customerAddressList = await _customer.GetAllCustomerAddresses(id);
                if (customerAddressList == null)
                    return NotFound($"Customer With ID {id} does not have any Addresses");
                return Ok(customerAddressList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/{id}/CustomerAddressID failed!", id);
                return BadRequest();
            }
        }

        // POST api/<CustomersController>
        [HttpPost("AddCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult<CustomerDTO>> Post(FullCustomerDTO newCustomer)
        {
            _logger.LogInformation(message: "POST: api/AddCustomer");
            if(!ValidateInput(newCustomer))
                return BadRequest("Invalid Data!");
            try
            {
                newCustomer.Password = clsUtil.ComputeHash(newCustomer.Password);
                var AddedCustomer = await _customer.AddNewCustomer(newCustomer);
                if (AddedCustomer != null)
                    return Ok(AddedCustomer);
                else
                    return BadRequest();
            }
            catch(Exception ex)
            {
                _logger.LogError(message: "The POST call to api/AddCustomer failed!");
                return BadRequest();
            }
        }

        [HttpPost("AddCustomerAddress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerAddressDTO>> Post(CustomerAddressDTO newCustomerAddress)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation(message: "POST: api/AddCustomerAddress");
            bool IsCustomerExist = await _customer.IsCustomerExist(newCustomerAddress.CustomerID);
            if (!IsCustomerExist || newCustomerAddress.AddressLine1 == "" ||
                newCustomerAddress.PostalCode == "" || newCustomerAddress.Country == "" || newCustomerAddress.City == "")
                return BadRequest("Invalid Data!");
            try
            {
                var AddedCustomerAddress = await _customer.AddNewCustomerAddress(newCustomerAddress);
                if (AddedCustomerAddress != null)
                    return Ok(AddedCustomerAddress);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The POST call to api/AddCustomerAddress failed!");
                return BadRequest();
            }
        }

        // PUT api/<CustomersController>/5
        [HttpPut("{id}/UpdatCustomer",Name ="UpdateCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id,CustomerDTO Updatedcustomer)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation("PUT: api/{id}/UpdateCustomer", id);
            if(id<1 ||!ValidateInput(Updatedcustomer))
                return BadRequest("Invalid Data");
            try
            {
                var customer = await _customer.Find(id);
                if (customer == null)
                    return NotFound($"User With ID {id} was not found");
                customer.FirstName = Updatedcustomer.FirstName;
                customer.LastName = Updatedcustomer.LastName;
                customer.Email = Updatedcustomer.Email;
                customer.Phone = Updatedcustomer.Phone;
                customer.Registered_At = Updatedcustomer.Registered_At;
                bool flag = await _customer.UpdateCustomer(customer);
                if (flag)
                    return Ok(customer);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/{id}/UpdateCustomer failed", id);
                return BadRequest();
            }
        }

        [HttpPut("{id}/UpdatCustomerPassword", Name = "UpdatCustomerPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id,string CurrentPassword,string NewPassword)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation("PUT: api/{id}/UpdatCustomerPassword", id);
            if (id < 1)
                return BadRequest("Invalid Data");
            try
            {
                var customer = await _customer.IsCustomerExist(id);
                if (!customer)
                    return NotFound($"User With ID {id} was not found");
                bool flag = await _customer.UpdateCustomerPassword(id,CurrentPassword,clsUtil.ComputeHash(NewPassword));
                if (flag)
                    return Ok("Your Password Has Been Updated Successfully!");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/{id}/UpdateCustomerPassword failed", id);
                return BadRequest();
            }
        }

        [HttpPut("{id}/UpdatCustomerAddress", Name = "UpdatCustomerAddress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, CustomerAddressDTO UpdatCustomerAddress)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation("PUT: api/{id}/UpdatCustomerAddress", id);
            bool IsCustomerExist = await _customer.IsCustomerExist(UpdatCustomerAddress.CustomerID);
            if (id <= 0 || IsCustomerExist || UpdatCustomerAddress.AddressLine1 == "" || UpdatCustomerAddress.PostalCode == ""
                || UpdatCustomerAddress.Country == "" || UpdatCustomerAddress.City == "")
                return BadRequest("Invalid Data!");
            try
            {
                var customerAddresses = await _customer.GetCustomerAddressByID(id);
                if (customerAddresses == null)
                    return NotFound($"CustomerAddress With ID {id} was not found");
                customerAddresses.CustomerID = UpdatCustomerAddress.CustomerID;
                customerAddresses.AddressLine1 = UpdatCustomerAddress.AddressLine1;
                customerAddresses.AddressLine2 = UpdatCustomerAddress.AddressLine2;
                customerAddresses.Country = UpdatCustomerAddress.Country;
                customerAddresses.City = UpdatCustomerAddress.City;
                customerAddresses.PostalCode = UpdatCustomerAddress.PostalCode;
                customerAddresses.PhoneNumber = UpdatCustomerAddress.PhoneNumber;

                bool flag = await _customer.UpdateCustomerAddress(customerAddresses);
                if (flag)
                    return Ok(customerAddresses);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/{id}/UpdatCustomerAddress failed", id);
                return BadRequest();
            }
        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{id}/DeleteCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation("DELETE: api/{id}/DeleteCustomer", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                bool flag = await _customer.DeleteCustomer(id);
                if (flag)
                    return Ok($"Customer with ID {id} was Deleted Successfully!");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError("the DELETE call to api/{id}/DeleteCustomer failed", id);
                return BadRequest();
            }
        }

        [HttpDelete("{id}/DeleteCustomerAddress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCustomerAddress(int id)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin &&
                (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation("DELETE: api/{id}/DeleteCustomerAddress", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                bool flag = await _customer.DeleteCustomerAddressByID(id);
                if (flag)
                    return Ok($"CustomerAddress with ID {id} was Deleted Successfully!");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"the DELETE call to api/{id}/DeleteCustomerAddress failed", id);
                return BadRequest();
            }
        }
        private bool ValidateInput(FullCustomerDTO newcustomer)
        {
            if (newcustomer == null || newcustomer.Password=="")
                return false;
            else if (!clsUtil.ValidateLettersOnly(newcustomer.FirstName) || !clsUtil.ValidateLettersOnly(newcustomer.LastName) ||
                !clsUtil.ValidateEmail(newcustomer.Email) || !clsUtil.ValidatePhoneNumber(newcustomer.Phone))
                return false;
            else
                return true;
        }
        private bool ValidateInput(CustomerDTO newcustomer)
        {
            if (newcustomer == null)
                return false;
            else if (!clsUtil.ValidateLettersOnly(newcustomer.FirstName) || !clsUtil.ValidateLettersOnly(newcustomer.LastName) || 
                !clsUtil.ValidateEmail(newcustomer.Email) || !clsUtil.ValidatePhoneNumber(newcustomer.Phone))
                return false;
            else
                return true;

        }
    }
}
