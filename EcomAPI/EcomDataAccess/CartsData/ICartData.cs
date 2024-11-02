
namespace EcomDataAccess.CartsData
{
    public interface ICartData
    {
        Task<int> AddNewCart(CartDTO cartDTO);
        Task<bool> DeleteCart(int CartID);
        Task<IList<CartDTO>> GetAllCarts();
        Task<CartDTO> GetCartByID(int CartID);
        Task<int> IsCartExist(int CardID);
        Task<bool> UpdateCrat(CartDTO cartDTO);
    }
}