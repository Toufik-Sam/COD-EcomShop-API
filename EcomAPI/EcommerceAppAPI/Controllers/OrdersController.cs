using EcomBusinessLayer.Customers;
using EcomBusinessLayer.Orders;
using EcomBusinessLayer.Products;
using EcomDataAccess;
using EcomDataAccess.OrdersData;
using EcomDataAccess.OrdersData.OrderItems;
using EcomDataAccess.OrdersData.OrderStatus;
using EcommerceAppAPI.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EcommerceAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrder _order;
        private readonly ICustomer _customer;
        private readonly IProduct _product;

        public OrdersController(ILogger<OrdersController>logger,IOrder order,ICustomer customer,IProduct product)
        {
            this._logger = logger;
            this._order = order;
            this._customer = customer;
            this._product = product;
        }

        // GET: api/<OrdersController>
        [HttpGet("AllCustomerOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<OrderDTO>>> GetAllCustomerOrders(int CustomerID)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin
              && (Global.User.Permission & Permissions.OrdersManger) != Permissions.OrdersManger && 
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/AllCustomerOrders");
            try
            {
                var OrdersList = await _order.GetAllCustomerOrders(CustomerID);
                return Ok(OrdersList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/AllCustomerOrders failed!");
                return BadRequest();
            }
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}/CustomerOrderByID",Name = "CustomerOrderByID")]
        public async Task<ActionResult<OrderDTO>> GetCustomerOrderByID(int id)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin
              && (Global.User.Permission & Permissions.OrdersManger) != Permissions.OrdersManger &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/{id}/CustomerOrderByID", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                OrderDTO orderDTO = await _order.Find(id);
                if (orderDTO == null)
                    return NotFound($"Customer Order With ID {id} was not found");
                return Ok(orderDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/{id}/CustomerOrderByID failed!", id);
                return BadRequest();
            }
        }

        [HttpGet("{id}/OrderItemByID",Name="OrderItemByID")]
        public async Task<ActionResult<OrderItemDTO>>GetOrderItemByID(int id)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin
              && (Global.User.Permission & Permissions.OrdersManger) != Permissions.OrdersManger &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation(message: "GET: api/{id}/OrderItemByID", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                OrderItemDTO OrderItemDTO = await _order.GetOrderItemByID(id);
                if (OrderItemDTO == null)
                    return NotFound($"Order Item With ID {id} was not found");
                return Ok(OrderItemDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/{id}/OrderItemByID failed!", id);
                return BadRequest();
            }
        }

        [HttpGet("AllOrderSatatuses", Name = "AllOrderSatatuses")]
        public async Task<ActionResult<OrderDTO>> GetAllOrderSatatuses()
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin
               && (Global.User.Permission & Permissions.OrdersManger) != Permissions.OrdersManger)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/AllOrderSatatuses)");

            try
            {
                var OrderStatusList = await _order.GetAllOrderSatatuses();
                return Ok(OrderStatusList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/AllOrderSatatuses failed!");
                return BadRequest();
            }
        }

        [HttpGet("{id}/OrderStatusByID", Name = "OrderStatusByID")]
        public async Task<ActionResult<OrderDTO>> GetOrderStatusByID(int id)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin
              && (Global.User.Permission & Permissions.OrdersManger) != Permissions.OrdersManger &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation(message: "GET: api/{id}/OrderStatusByID", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                OrderStatusDTO orderStatusDTO = await _order.GetOrderStatusByID(id);
                if (orderStatusDTO == null)
                    return NotFound($"Order Status With ID {id} was not found");
                return Ok(orderStatusDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/{id}/OrderStatusByID failed!", id);
                return BadRequest();
            }
        }

        [HttpGet("{id}/AllOrderItems", Name = "AllOrderItems")]
        public async Task<ActionResult<OrderDTO>> GetOrderItemsByID(int id)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin
              && (Global.User.Permission & Permissions.OrdersManger) != Permissions.OrdersManger &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation(message: "GET: api/{id}/AllOrderItems", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                var orderItemsList = await _order.GetAllOrderItems(id);
                if (orderItemsList == null)
                    return BadRequest();
                return Ok(orderItemsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/{id}/AllOrderItems failed!", id);
                return BadRequest();
            }
        }

        // POST api/<OrdersController>
        [HttpPost("AddNewOrder")]
        public async Task<ActionResult<OrderDTO>> Post(OrderDTO orderDTO)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin
              && (Global.User.Permission & Permissions.OrdersManger) != Permissions.OrdersManger &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation(message: "POST: api/AddedOrder");
            bool IsOrderExist = await _order.IsOrderExist(orderDTO.OrderID);
            bool IsCustomerExist = await _customer.IsCustomerExist(orderDTO.OrderID);
            if (!IsOrderExist && !IsCustomerExist && orderDTO.OrderStatusID<1)
                return BadRequest("Invalid Data!");
            try
            {
                var AddedOrder = await _order.AddNewOrder(orderDTO);
                if (AddedOrder != null)
                    return Ok(AddedOrder);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,message: "The POST call to api/AddedOrder failed!");
                return BadRequest();
            }
        }

        [HttpPost("AddNewOrderStatus")]
        public async Task<ActionResult<OrderStatusDTO>> Post(OrderStatusDTO orderStatusDTO)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin
             && (Global.User.Permission & Permissions.OrdersManger) != Permissions.OrdersManger)
                return Unauthorized();
            _logger.LogInformation(message: "POST: api/AddNewOrderStatus");
            if (orderStatusDTO.StatusName == "")
                return BadRequest("Invalid Data");
            try
            {
                var AddedOrderStatus = await _order.AddNewOrderStatus(orderStatusDTO);
                if (AddedOrderStatus != null)
                    return Ok(AddedOrderStatus);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(message: "The POST call to api/AddNewOrderStatus failed!");
                return BadRequest();
            }
        }

        [HttpPost("AddNewOrderItem")]
        public async Task<ActionResult<OrderItemDTO>>Post(OrderItemDTO orderItemDTO)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin
              && (Global.User.Permission & Permissions.OrdersManger) != Permissions.OrdersManger &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation(message: "POST: api/AddNewOrderItem");
            bool IsProductExist = await _product.IsProductExist(orderItemDTO.ProductID);
            bool IsOrderExist = await _order.IsOrderExist(orderItemDTO.OrderID);
            if (!IsOrderExist || !IsProductExist && orderItemDTO.Price<=0 || orderItemDTO.Quantity<=0)
                return BadRequest("Invalid Data!");
            try
            {
                var AddedOrderItem = await _order.AddNewOrderItem(orderItemDTO);
                if (AddedOrderItem != null)
                    return Ok(AddedOrderItem);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The POST call to api/AddNewOrderItem failed!");
                return BadRequest();
            }
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}/ShipOrder")]
        public async Task<IActionResult> ShipOrder(int id)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin
              && (Global.User.Permission & Permissions.OrdersManger) != Permissions.OrdersManger &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation("PUT: api/{id}/ShipOrder", id);
            bool IsOrderExist = await _order.IsOrderExist(id);
            if (!IsOrderExist)
                return BadRequest("Invalid Data!");
            try
            {
                bool flag = await _order.ShipOrder(id);
                if (flag)
                    return Ok("Order Shipped Successfully!");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/{id}/ShipOrder failed", id);
                return BadRequest();
            }
        }

        [HttpPut("{id}/DeliverOrder")]
        public async Task<IActionResult> DeliverOrder(int id)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin
              && (Global.User.Permission & Permissions.OrdersManger) != Permissions.OrdersManger &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation("PUT: api/{id}/DeliverOrder", id);
            bool IsOrderExist = await _order.IsOrderExist(id);
            if (!IsOrderExist)
                return BadRequest("Invalid Data!");
            try
            {
                bool flag = await _order.DeliverOrder(id);
                if (flag)
                    return Ok("Order Delivered Successfully!");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/{id}/DeliverOrder failed", id);
                return BadRequest();
            }
        }

        [HttpPut("{id}/ConfirmOrder")]
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin
              && (Global.User.Permission & Permissions.OrdersManger) != Permissions.OrdersManger &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation("PUT: api/{id}/ConfirmOrder", id);
            bool IsOrderExist = await _order.IsOrderExist(id);
            if (!IsOrderExist)
                return BadRequest("Invalid Data!");
            try
            {
                bool flag = await _order.ConfirmOrder(id);
                if (flag)
                    return Ok("Order Confirmed Successfully!");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/{id}/ConfirmOrder failed", id);
                return BadRequest();
            }
        }

        [HttpPut("{id}/CancelOrder")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin
              && (Global.User.Permission & Permissions.OrdersManger) != Permissions.OrdersManger &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation("PUT: api/{id}/CancelOrder", id);
            bool IsOrderExist = await _order.IsOrderExist(id);
            if (!IsOrderExist)
                return BadRequest("Invalid Data!");
            try
            {
                bool flag = await _order.CancelOrder(id);
                if (flag)
                    return Ok("Order Canceled Successfully!");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/{id}/CancelOrder failed", id);
                return BadRequest();
            }
        }

        [HttpPut("{id}/ReturnOrder")]
        public async Task<IActionResult> ReturnOrder(int id)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin
              && (Global.User.Permission & Permissions.OrdersManger) != Permissions.OrdersManger &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation("PUT: api/{id}/ReturnOrder", id);
            bool IsOrderExist = await _order.IsOrderExist(id);
            if (!IsOrderExist)
                return BadRequest("Invalid Data!");
            try
            {
                bool flag = await _order.ReturnOrder(id);
                if (flag)
                    return Ok("Order Return Successfully!");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/{id}/ReturnOrder failed", id);
                return BadRequest();
            }
        }

        [HttpPut("{id}/UpdateOrderStatus")]
        public async Task<IActionResult>Put(int id, OrderStatusDTO UpdatedOrderStatusDTO)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin
               && (Global.User.Permission & Permissions.OrdersManger) != Permissions.OrdersManger)
                return Unauthorized();
            _logger.LogInformation("PUT: api/{id}/UpdateOrderStatus", id);

            if (UpdatedOrderStatusDTO.StatusName=="")
                return BadRequest("Invalid Data!");
            try
            {
                OrderStatusDTO orderStatus = await _order.GetOrderStatusByID(id);
                if (orderStatus == null)
                    return NotFound($"Order Status With ID {id} was not found");
                orderStatus.StatusName = UpdatedOrderStatusDTO.StatusName;
                bool flag = await _order.UpdateOrderStatus(orderStatus);
                if (flag)
                    return Ok(orderStatus);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/{id}/UpdateOrderStatus failed", id);
                return BadRequest();
            }
        }

        [HttpPut("{id}/UpdateOrderItem")]
        public async Task<IActionResult> Put(int id, OrderItemDTO UpdatedorderItemDTO)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin
              && (Global.User.Permission & Permissions.OrdersManger) != Permissions.OrdersManger &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation("PUT: api/{id}/UpdateOrderItem", id);
            bool IsProductExist = await _product.IsProductExist(UpdatedorderItemDTO.ProductID);
            bool IsOrderExist = await _order.IsOrderExist(UpdatedorderItemDTO.OrderID);
            if (!IsOrderExist || !IsOrderExist || UpdatedorderItemDTO.Price <= 0|| UpdatedorderItemDTO.Quantity<=0)
                return BadRequest("Invalid Data!");
            try
            {
                var OrderItemDTO = await _order.GetOrderItemByID(id);
                if (OrderItemDTO == null)
                    return NotFound($"OrderItem With ID {id} was not found");
                OrderItemDTO.OrderID = UpdatedorderItemDTO.OrderID;
                OrderItemDTO.ProductID = UpdatedorderItemDTO.ProductID;
                OrderItemDTO.Price = UpdatedorderItemDTO.Price;
                OrderItemDTO.Quantity = UpdatedorderItemDTO.Quantity;
                bool flag = await _order.UpdateOrderItem(OrderItemDTO);
                if (flag)
                    return Ok(OrderItemDTO);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/{id}/UpdateOrderItem failed", id);
                return BadRequest();
            }
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}/DeleteOrderByID")]
        public async Task<IActionResult> DeleteOrderByID(int id)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin
              && (Global.User.Permission & Permissions.OrdersManger) != Permissions.OrdersManger &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation("DELETE: api/{id}/DeleteOrderByID", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                bool flag = await _order.DeleteOrderByID(id);
                if (flag)
                    return Ok($"Order with ID {id} was Deleted Successfully!");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError("the DELETE call to api/{id}/DeleteOrder failed", id);
                return BadRequest();
            }
        }

        [HttpDelete("{id}/DeleteOrderStatusByID")]
        public async Task<IActionResult> DeleteOrderStatusByID(int id)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin
             && (Global.User.Permission & Permissions.OrdersManger) != Permissions.OrdersManger)
                return Unauthorized();
            _logger.LogInformation("DELETE: api/{id}/DeleteOrderStatusByID", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                bool flag = await _order.DeleteOrderStatusByID(id);
                if (flag)
                    return Ok($"Order Status with ID {id} was Deleted Successfully!");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError("the DELETE call to api/{id}/DeleteOrderStatusByID failed", id);
                return BadRequest();
            }
        }

        [HttpDelete("{id}/DeleteOrderItemID")]
        public async Task<IActionResult> DeleteOrderItemID(int id)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin
              && (Global.User.Permission & Permissions.OrdersManger) != Permissions.OrdersManger &&
              (Global.User.Permission & Permissions.Customer) != Permissions.Customer)
                return Unauthorized();

            _logger.LogInformation("DELETE: api/{id}/DeleteOrderItemID", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                bool flag = await _order.DeleteOrderItem(id);
                if (flag)
                    return Ok($"Order Status with ID {id} was Deleted Successfully!");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError("the DELETE call to api/{id}/DeleteOrderItemID failed", id);
                return BadRequest();
            }
        }
    }
}
