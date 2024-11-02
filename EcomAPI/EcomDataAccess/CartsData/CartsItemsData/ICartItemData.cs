
namespace EcomDataAccess.CartsData.CartsItemsData
{
    public interface ICartItemData
    {
        Task<int> AddNewCartItem(CartItemDTO cartItemDTO);
        Task<bool> DeleteCartItem(int CartItemID);
        Task<IList<CartItemDTO>> GetAllCardItems(int CartID);
        Task<CartItemDTO> GetCartItemByID(int CartItemID);
        Task<bool> UpdateCartItem(CartItemDTO cartItemDTO);
    }
}