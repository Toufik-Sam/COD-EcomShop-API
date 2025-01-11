using EcomBusinessLayer.Cards;
using EcomBusinessLayer.Customers;
using EcomBusinessLayer.Products;
using EcomDataAccess.CartsData.CartsItemsData;
using EcomDataAccess.CartsData;
using Microsoft.AspNetCore.Mvc;
using EcomDataAccess;
using EcommerceAppAPI.Models;
using System.Security;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EcommerceAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly IProduct _product;
        private readonly ICustomer _customer;
        private readonly ICart _cart;
        private readonly ILogger<CartsController> _logger;
        private readonly IUserService _global;

        public CartsController(IProduct product, ICustomer customer, ICart cart, ILogger<CartsController> logger,IUserService global)
        {
            this._product = product;
            this._customer = customer;
            this._cart = cart;
            this._logger = logger;
            this._global = global;
            this._customer = customer;
        }
        // GET: api/<CardsController>
        [HttpGet("AllCarts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CartDTO>> GetAllCarts()
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/AllCarts");
            try
            {
                var CartList = await _cart.GetAllCarts();
                return Ok(CartList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/AllCards failed!");
                return BadRequest();
            }
        }

        [HttpGet("AllCartItems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<CartItemDTO>>> GetAllCartItems(int CardID)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin &&
              (_global.GetUser().Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/AllCartItems");
            try
            {
                var CartItemsList = await _cart.GetAllCartItems(CardID);
                return Ok(CartItemsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/AllCartItems failed!");
                return BadRequest();
            }
        }

        // GET api/<CardsController>/5
        [HttpGet("{id}/CartByID", Name = "CartByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CartDTO>> GetCartByID(int id)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin &&
              (_global.GetUser().Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/{id}/CartByID", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                CartDTO cart = await _cart.Find(id);
                if (cart == null)
                    return NotFound($"Cart With ID {id} was not found");
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/{id}/CartByID failed!", id);
                return BadRequest();
            }
        }

        [HttpGet("{id}/CartItemByID", Name = "CartItemByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CartItemDTO>> GetCartItemByID(int id)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin &&
               (_global.GetUser().Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/{id}/CartItemByID", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                CartItemDTO cartItem = await _cart.GetCartItemByID(id);
                if (cartItem == null)
                    return NotFound($"Cart Item With ID {id} was not found");
                return Ok(cartItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/{id}/CartItemByID failed!", id);
                return BadRequest();
            }
        }

        // POST api/<CardsController>
        [HttpPost("AddNewcart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CartDTO>> Post(CartDTO cartDTO)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin &&
              (_global.GetUser().Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();
            _logger.LogInformation(message: "POST: api/AddNewCart");
            var IsCustomerExist = await _customer.IsCustomerExist(cartDTO.CustomerID);
            if (!IsCustomerExist)
                return BadRequest("Invalid Data!");
            try
            {
                var AddedCart = await _cart.AddNewCrat(cartDTO);
                if (AddedCart != null)
                    return Ok(AddedCart);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(message: "The POST call to api/AddedCart failed!");
                return BadRequest();
            }
        }

        [HttpPost("AddNewCartItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CartItemDTO>> Post(CartItemDTO cartItemDTO)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin &&
                (_global.GetUser().Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();
            _logger.LogInformation(message: "POST: api/AddNewCartItem");
            var IsCartExist = await _cart.IsCartExist(cartItemDTO.CartID);
            var IsProductExist = await _product.IsProductExist(cartItemDTO.ProductID);
            if (!IsCartExist || !IsProductExist || cartItemDTO.Quantity <= 0)
                return BadRequest("Invalid Data!");
            try
            {
                var AddedCartItem = await _cart.AddNewCartItem(cartItemDTO);
                if (AddedCartItem != null)
                    return Ok(AddedCartItem);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(message: "The POST call to api/AddNewCartItem failed!");
                return BadRequest();
            }
        }

        // PUT api/<CardsController>/5
        [HttpPut("{id}/UpdateCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, CartDTO UpdatedCart)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin &&
               (_global.GetUser().Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();
            _logger.LogInformation("PUT: api/{id}/UpdateCart", id);
            var IsCustomerExist = await _customer.IsCustomerExist(UpdatedCart.CustomerID);
            if (!IsCustomerExist)
                return BadRequest("Invalid Data");
            try
            {
                var cart = await _cart.Find(id);
                if (cart == null)
                    return NotFound($"Cart With ID {id} was not found");
                cart.CustomerID = UpdatedCart.CustomerID;
                bool flag = await _cart.UpdateCart(cart);
                if (flag)
                    return Ok(cart);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/{id}/UpdateCart failed", id);
                return BadRequest();
            }
        }

        [HttpPut("{id}/UpdateCartItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, CartItemDTO UpdatedCartItem)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin &&
               (_global.GetUser().Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();
            _logger.LogInformation("PUT: api/{id}/UpdateCartItem", id);
            var IsCartExist = await _cart.IsCartExist(UpdatedCartItem.CartID);
            var IsProductExist = await _product.IsProductExist(UpdatedCartItem.ProductID);
            if (!IsCartExist || !IsProductExist)
                return BadRequest("Invalid Data");
            try
            {
                var cartItem = await _cart.GetCartItemByID(id);
                if (cartItem == null)
                    return NotFound($"Cart Item With ID {id} was not found");
                cartItem.CartID = UpdatedCartItem.CartID;
                cartItem.ProductID = UpdatedCartItem.ProductID;
                cartItem.Quantity = UpdatedCartItem.Quantity;
                bool flag = await _cart.UpdateCartItem(cartItem);
                if (flag)
                    return Ok(cartItem);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/{id}/UpdateCartItem failed", id);
                return BadRequest();
            }
        }

        // DELETE api/<CardsController>/5
        [HttpDelete("{id}/DeleteCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCart(int id)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin &&
               (_global.GetUser().Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();
            _logger.LogInformation("DELETE: api/{id}/DeleteCart", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                bool flag = await _cart.DeleteCart(id);
                if (flag)
                    return Ok($"Cart with ID {id} was Deleted Successfully!");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "the DELETE call to api/{id}/DeleteCart failed", id);
                return BadRequest();
            }
        }

        [HttpDelete("{id}/DeleteCartItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin &&
                 (_global.GetUser().Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();
            _logger.LogInformation("DELETE: api/{id}/DeleteCartItem", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                bool flag = await _cart.DeleteCartItem(id);
                if (flag)
                    return Ok($"Cart Item with ID {id} was Deleted Successfully!");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "the DELETE call to api/{id}/DeleteCartItem failed", id);
                return BadRequest();
            }
        }
    }
}
