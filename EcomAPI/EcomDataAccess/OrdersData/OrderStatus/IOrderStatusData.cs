
namespace EcomDataAccess.OrdersData.OrderStatus
{
    public interface IOrderStatusData
    {
        Task<int> AddNewOrderStatus(OrderStatusDTO orderStatusDTO);
        Task<bool> DeleteOrderStatus(int OrderStatusID);
        Task<IList<OrderStatusDTO>> GetAllOrderStatuses();
        Task<OrderStatusDTO> GetOrderStatusByID(int OrderStatusID);
        Task<bool> UpdateOrderStatus(OrderStatusDTO orderStatusDTO);
    }
}