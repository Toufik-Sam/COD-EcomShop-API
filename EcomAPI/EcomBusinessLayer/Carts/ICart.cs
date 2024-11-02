using EcomDataAccess.CartsData;
using EcomDataAccess.CartsData.CartsItemsData;

namespace EcomBusinessLayer.Cards
{
    public interface ICart
    {
        Task<CartItemDTO> AddNewCartItem(CartItemDTO cartItemDTO);
        Task<CartDTO> AddNewCrat(CartDTO cartDTO);
        Task<bool> DeleteCart(int CartID);
        Task<bool> DeleteCartItem(int CartItemID);
        Task<CartDTO> Find(int CartID);
        Task<IList<CartItemDTO>> GetAllCartItems(int CartID);
        Task<IList<CartDTO>> GetAllCarts();
        Task<CartItemDTO> GetCartItemByID(int CartItemID);
        Task<bool> IsCartExist(int CartID);
        Task<bool> UpdateCart(CartDTO cartDTO);
        Task<bool> UpdateCartItem(CartItemDTO cartItemDTO);
    }
}