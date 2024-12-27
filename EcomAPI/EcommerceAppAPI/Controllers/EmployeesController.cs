using EcomBusinessLayer.Employees;
using EcomDataAccess;
using EcomDataAccess.EmployeesData;
using EcommerceAppAPI.Models;
using EcommerceAppAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EcommerceAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployee _employee;
        private readonly ILogger<EmployeesController> _logger;
        private readonly IUserService _global;

        public EmployeesController(IEmployee employee,ILogger<EmployeesController>logger,IUserService global)
        {
            this._employee = employee;
            this._logger = logger;
            this._global = global;
        }
        // GET: api/<EmployeesController>
        [HttpGet("AllEmployess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<EmployeeDTO>>>Get()
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/AllCustomers");
            try
            {
                var CustomersList = await _employee.GetAllEmployees();
                return Ok(CustomersList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/AllCustomers failed!");
                return BadRequest();
            }
        }

        // GET api/<EmployeesController>/5
        [HttpGet("{id}/EmployeeByID",Name ="GetEmployeeByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeDTO>> GetÊmployeeByID(int id)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/{id}/EmployeeByID", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                EmployeeDTO employee = await _employee.Find(id);
                if (employee == null)
                    return NotFound($"Employee With ID {id} was not found");
                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/{id}/EmployeeByID failed!", id);
                return BadRequest();
            }
        }

        // POST api/<EmployeesController>
        [HttpPost("AddEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeDTO>> Post(FullEmployeeDTO employee)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin)
                return Unauthorized();
            _logger.LogInformation(message: "POST: api/AddCustomer");
            if (!ValidateInput(employee))
                return BadRequest("Invalid Data!");
            try
            {
                var AddedEmployee = await _employee.AddNewEmployee(employee);
                if (AddedEmployee != null)
                    return Ok(AddedEmployee);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(message: "The POST call to api/AddCustomer failed!");
                return BadRequest();
            }
        }

        // PUT api/<EmployeesController>/5
        [HttpPut("{id}/UpdateEmployee", Name = "UpdateEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, EmployeeDTO Updatedemployee)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin)
                return Unauthorized();
            _logger.LogInformation("PUT: api/{id}/UpdateEmployee", id);
            if (id < 1 || !ValidateInput(Updatedemployee))
                return BadRequest("Invalid Data");
            try
            {
                var employee = await _employee.Find(id);
                if (employee == null)
                    return NotFound($"User With ID {id} was not found");
                employee.FirstName = Updatedemployee.FirstName;
                employee.LastName = Updatedemployee.LastName;
                employee.Email = Updatedemployee.Email;
                employee.Phone = Updatedemployee.Phone;
                employee.Registered_At = Updatedemployee.Registered_At;
                bool flag = await _employee.UpdateEmployee(employee);
                if (flag)
                    return Ok(employee);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/{id}/UpdateCustomer failed", id);
                return BadRequest();
            }
        }

        [HttpPut("{id}/UpdatEmployeePassword", Name = "UpdatEmployeePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, string CurrentPassword, string NewPassword)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin &&
              (_global.GetUser().Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation("PUT: api/{id}/UpdatEmployeePassword", id);
            if (id < 1)
                return BadRequest("Invalid Data");
            try
            {
                var customer = await _employee.IsEmployeeExist(id);
                if (!customer)
                    return NotFound($"User With ID {id} was not found");
                bool flag = await _employee.UpdateEmployeePassword(id, CurrentPassword, clsUtil.ComputeHash(NewPassword));
                if (flag)
                    return Ok("Your Password Has Been Updated Successfully!");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/{id}/UpdatEmployeePassword failed", id);
                return BadRequest();
            }
        }

        // DELETE api/<EmployeesController>/5
        [HttpDelete("{id}/DeleteEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin)
                return Unauthorized();
            _logger.LogInformation("DELETE: api/{id}/DeleteEmployee", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                bool flag = await _employee.DeleteEmployee(id);
                if (flag)
                    return Ok($"Employee with ID {id} was Deleted Successfully!");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError("the DELETE call to api/{id}/DeleteEmployee failed", id);
                return BadRequest();
            }
        }
        private bool IsValidEmail(string source)
        {
            return new EmailAddressAttribute().IsValid(source);
        }
        private bool ValidateInput(FullEmployeeDTO newEmployee)
        {
            if (newEmployee == null || newEmployee.Password=="" || (newEmployee.PermissionLevel & Permissions.Customer)
                == Permissions.Customer)
                return false;
            else if (!clsUtil.ValidateLettersOnly(newEmployee.FirstName) || !clsUtil.ValidateLettersOnly(newEmployee.LastName) ||
                !clsUtil.ValidateEmail(newEmployee.Email) || !clsUtil.ValidatePhoneNumber(newEmployee.Phone))
                return false;
            else
                return true;
        }
        private bool ValidateInput(EmployeeDTO newEmployee)
        {
            if (newEmployee == null || (newEmployee.PermissionLevel & Permissions.Customer) == Permissions.Customer)
                return false;
            else if (!clsUtil.ValidateLettersOnly(newEmployee.FirstName) || !clsUtil.ValidateLettersOnly(newEmployee.LastName) ||
                !clsUtil.ValidateEmail(newEmployee.Email) || !clsUtil.ValidatePhoneNumber(newEmployee.Phone))
                return false;
            else
                return true;
        }
    }
}
