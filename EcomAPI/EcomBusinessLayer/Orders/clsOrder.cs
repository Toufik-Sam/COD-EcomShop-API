using EcomDataAccess.OrdersData;
using EcomDataAccess.OrdersData.OrderItems;
using EcomDataAccess.OrdersData.OrderStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomBusinessLayer.Orders
{
    public class clsOrder : IOrder
    {
        private readonly IOrdersData _orderData;
        private readonly IOrderStatusData _orderStatusData;
        private readonly IOrderItemsData _orderItemsData;

        public clsOrder(IOrdersData orderData, IOrderStatusData orderStatusData,IOrderItemsData orderItemsData)
        {
            this._orderData = orderData;
            this._orderStatusData = orderStatusData;
            this._orderItemsData = orderItemsData;
        }
        public async Task<OrderDTO> AddNewOrder(OrderDTO orderDTO)
        {
            int newID = await _orderData.AddNewOrder(orderDTO);
            return newID > 0 ? new OrderDTO(newID, orderDTO.CustomerID,
                orderDTO.OrderStatusID, orderDTO.CreatedAt) : null!;
        }
        public async Task<bool> UpdateOrder(OrderDTO orderDTO)
        {
            bool flag = await _orderData.UpdateOrder(orderDTO);
            return flag;
        }
        public async Task<IList<OrderDTO>> GetAllCustomerOrders(int CustomerID)
        {
            var CustomerOrdersList = await _orderData.GetAllCustomerOrders(CustomerID);
            return CustomerOrdersList;
        }
        public async Task<OrderDTO> Find(int OrderID)
        {
            var Order = await _orderData.GetOrderByID(OrderID);
            return Order;
        }
        public async Task<bool> IsOrderExist(int OrderID)
        {
            int Found = await _orderData.IsOrderExist(OrderID);
            return Found != -1;
        }
        public async Task<bool> DeleteOrderByID(int OrderID)
        {
            bool flag = await _orderData.DeleteOrderByID(OrderID);
            return flag;
        }
        public async Task<OrderStatusDTO> AddNewOrderStatus(OrderStatusDTO orderStatusDTO)
        {
            int newID = await _orderStatusData.AddNewOrderStatus(orderStatusDTO);
            return newID > 0 ? new OrderStatusDTO(orderStatusDTO.OrderStatusID, orderStatusDTO.StatusName) : null!;
        }
        public async Task<bool> UpdateOrderStatus(OrderStatusDTO orderStatusID)
        {
            bool flag = await _orderStatusData.UpdateOrderStatus(orderStatusID);
            return flag;
        }
        public async Task<IList<OrderStatusDTO>> GetAllOrderSatatuses()
        {
            var OrderStatusesList = await _orderStatusData.GetAllOrderStatuses();
            return OrderStatusesList;
        }
        public async Task<OrderStatusDTO> GetOrderStatusByID(int OrderStatusID)
        {
            var OrderStatus = await _orderStatusData.GetOrderStatusByID(OrderStatusID);
            return OrderStatus;
        }
        public async Task<bool> DeleteOrderStatusByID(int OrderStatusID)
        {
            bool flag = await _orderStatusData.DeleteOrderStatus(OrderStatusID);
            return flag;
        }
        public async Task<OrderItemDTO>AddNewOrderItem(OrderItemDTO orderItemDTO)
        {
            int newID = await _orderItemsData.AddNewOrderItem(orderItemDTO);
            return newID > 0 ? new OrderItemDTO(newID, orderItemDTO.ProductID,
                orderItemDTO.OrderID, orderItemDTO.Price, orderItemDTO.Quantity):null!;
        }
        public async Task<bool>UpdateOrderItem(OrderItemDTO orderItemDTO)
        {
            bool flag = await _orderItemsData.UpdateOrderItem(orderItemDTO);
            return flag;
        }
        public async Task<IList<OrderItemDTO>>GetAllOrderItems(int OrderID)
        {
            var OrderItemsList = await _orderItemsData.GetAllOrderItems(OrderID);
            return OrderItemsList;
        }
        public async Task<OrderItemDTO>GetOrderItemByID(int OrderItemID)
        {
            var OrderItem = await _orderItemsData.GetOrderItemByID(OrderItemID);
            return OrderItem;
        }
        public async Task<bool>DeleteOrderItem(int OrderItem)
        {
            bool flag = await _orderItemsData.DeleteOrderItem(OrderItem);
            return flag;
        }
    }

}
