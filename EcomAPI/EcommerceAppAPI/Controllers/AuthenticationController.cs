using EcomBusinessLayer.Customers;
using EcomBusinessLayer.Employees;
using EcomDataAccess;
using EcomDataAccess.CustomersData;
using EcomDataAccess.EmployeesData;
using EcommerceAppAPI.Models;
using EcommerceAppAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcommerceAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ICustomer _customer;
        private readonly IEmployee _employee;
        private readonly IUserService _global;

        public AuthenticationController(IConfiguration config, ICustomer customer, IEmployee employee, IUserService global)
        {
            this._config = config;
            this._customer = customer;
            this._employee = employee;
            this._global = global;
        }
        public record AuthenticationData(string?Email,string?PassWord);
        //api/Authentication/tokren
        [HttpPost("token")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<clsUser>> Authenticate([FromBody]AuthenticationData Data)
        {
            _global.SetUser(await ValidateCredentials(Data));
            if (_global.GetUser() == null)
                return Unauthorized();
            var token = GenerateToken(_global.GetUser());
            _global.GetUser().Token = token;
            return Ok(_global.GetUser());
        }
        private  string GenerateToken(clsUser User)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.
                GetBytes(_config.GetValue<string>(key: "Authentication:SecretKey")!));

            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new();
            claims.Add(new(JwtRegisteredClaimNames.Sub, User.UserID.ToString()));
            claims.Add(new(JwtRegisteredClaimNames.FamilyName, User.LastName));
            claims.Add(new(JwtRegisteredClaimNames.GivenName, User.FirstName));
            claims.Add(new(JwtRegisteredClaimNames.Email, User.Email));
            claims.Add(new(JwtRegisteredClaimNames.PhoneNumber, User.Phone));

            var token = new JwtSecurityToken(
                _config.GetValue<string>(key: "Authentication:Issuer"),
                _config.GetValue<string>(key: "Authentication:Audience"),
                claims,
                DateTime.UtcNow, //when this token becomes valid
                DateTime.UtcNow.AddMinutes(value: 1),// when the token will Expire
                signingCredentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private async Task<clsUser> ValidateCredentials(AuthenticationData Data)
        {
            CustomerDTO customer = await _customer.Find(Data.Email,clsUtil.ComputeHash(Data.PassWord));
            if(customer!=null)
                return new clsUser(customer.CustomerID, customer.LastName, customer.FirstName, customer.Email, customer.Phone, 
                    Permissions.Customer, "");
            EmployeeDTO employee = await _employee.Find(Data.Email, clsUtil.ComputeHash(Data.PassWord));
            if(employee!=null)
                return new clsUser(employee.EmployeeID, employee.LastName, employee.FirstName, employee.Email, employee.Phone,
                (Permissions)employee.PermissionLevel, "");
            return null;

        }
    }
}
