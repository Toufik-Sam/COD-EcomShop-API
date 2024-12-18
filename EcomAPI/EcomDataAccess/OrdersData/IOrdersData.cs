namespace EcomDataAccess.OrdersData
{
    public interface IOrdersData
    {
        Task<int> AddNewOrder(OrderDTO orderDTO);
        Task<bool> DeleteOrderByID(int OrderID);
        Task<IList<OrderDTO>> GetAllCustomerOrders(int CustomerID);
        Task<OrderDTO> GetOrderByID(int OrderID);
        Task<int> IsOrderExist(int OrderID);
        Task<bool> ShipOrder(int OrderID);
        Task<bool> DeliverOrder(int OrderID);
        Task<bool> ConfirmOrder(int OrderID);
        Task<bool> CancelOrder(int OrderID);
        Task<bool> ReturnOrder(int OrderID);
    }
}