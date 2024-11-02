
namespace EcomDataAccess.OrdersData.OrderItems
{
    public interface IOrderItemsData
    {
        Task<int> AddNewOrderItem(OrderItemDTO orderItem);
        Task<bool> DeleteOrderItem(int OrderItemID);
        Task<OrderItemDTO> GetOrderItemByID(int OrderItemID);
        Task<IList<OrderItemDTO>> GetAllOrderItems(int OrderID);
        Task<bool> UpdateOrderItem(OrderItemDTO orderItem);
    }
}