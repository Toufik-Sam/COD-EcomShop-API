
namespace EcomDataAccess.OrdersData
{
    public interface IOrdersData
    {
        Task<int> AddNewOrder(OrderDTO orderDTO);
        Task<bool> DeleteOrderByID(int OrderID);
        Task<IList<OrderDTO>> GetAllCustomerOrders(int CustomerID);
        Task<OrderDTO> GetOrderByID(int OrderID);
        Task<int> IsOrderExist(int OrderID);
        Task<bool> UpdateOrder(OrderDTO orderDTO);

    }
}