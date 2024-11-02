using EcomDataAccess.OrdersData;
using EcomDataAccess.OrdersData.OrderItems;
using EcomDataAccess.OrdersData.OrderStatus;

namespace EcomBusinessLayer.Orders
{
    public interface IOrder
    {
        Task<OrderDTO> AddNewOrder(OrderDTO orderDTO);
        Task<OrderStatusDTO> AddNewOrderStatus(OrderStatusDTO orderStatusDTO);
        Task<bool> DeleteOrderByID(int OrderID);
        Task<bool> DeleteOrderStatusByID(int OrderStatusID);
        Task<OrderDTO> Find(int OrderID);
        Task<IList<OrderDTO>> GetAllCustomerOrders(int CustomerID);
        Task<IList<OrderStatusDTO>> GetAllOrderSatatuses();
        Task<OrderStatusDTO> GetOrderStatusByID(int OrderStatusID);
        Task<bool> IsOrderExist(int OrderID);
        Task<bool> UpdateOrder(OrderDTO orderDTO);
        Task<bool> UpdateOrderStatus(OrderStatusDTO orderStatusID);
        Task<OrderItemDTO> AddNewOrderItem(OrderItemDTO orderItemDTO);
        Task<bool> UpdateOrderItem(OrderItemDTO orderItemDTO);
        Task<IList<OrderItemDTO>> GetAllOrderItems(int OrderID);
        Task<bool> DeleteOrderItem(int OrderItemID);
        Task<OrderItemDTO> GetOrderItemByID(int OrderItemID);
    }
}