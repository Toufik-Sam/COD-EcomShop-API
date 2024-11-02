using EcomDataAccess.CartsData;
using EcomDataAccess.CartsData.CartsItemsData;
namespace EcomBusinessLayer.Cards
{
    public class clsCart : ICart
    {
        private readonly ICartData _cartData;
        private readonly ICartItemData _cartItemData;

        public clsCart(ICartData cartData, ICartItemData cartItemData)
        {
            this._cartData = cartData;
            this._cartItemData = cartItemData;
        }
        public async Task<CartDTO> AddNewCrat(CartDTO cartDTO)
        {
            int newID = await _cartData.AddNewCart(cartDTO);
            return newID > 0 ? new CartDTO(newID, cartDTO.CustomerID) : null!;
        }
        public async Task<bool> UpdateCart(CartDTO cartDTO)
        {
            bool flag = await _cartData.UpdateCrat(cartDTO);
            return flag;
        }
        public async Task<IList<CartDTO>> GetAllCarts()
        {
            var CartsList = await _cartData.GetAllCarts();
            return CartsList;
        }
        public async Task<CartDTO> Find(int CartID)
        {
            var Cart = await _cartData.GetCartByID(CartID);
            return Cart;
        }
        public async Task<bool> DeleteCart(int CartID)
        {
            bool flag = await _cartData.DeleteCart(CartID);
            return flag;
        }
        public async Task<bool> IsCartExist(int CartID)
        {
            int Found = await _cartData.IsCartExist(CartID);
            return Found != -1;
        }
        public async Task<CartItemDTO> AddNewCartItem(CartItemDTO cartItemDTO)
        {
            int newID = await _cartItemData.AddNewCartItem(cartItemDTO);
            return newID > 0 ? new CartItemDTO(newID, cartItemDTO.CartID, cartItemDTO.ProductID, cartItemDTO.Quantity) : null!;
        }
        public async Task<bool> UpdateCartItem(CartItemDTO cartItemDTO)
        {
            bool flag = await _cartItemData.UpdateCartItem(cartItemDTO);
            return flag;
        }
        public async Task<CartItemDTO> GetCartItemByID(int CartItemID)
        {
            var CartItem = await _cartItemData.GetCartItemByID(CartItemID);
            return CartItem;
        }
        public async Task<IList<CartItemDTO>> GetAllCartItems(int CartID)
        {
            var CartItemsList = await _cartItemData.GetAllCardItems(CartID);
            return CartItemsList;
        }
        public async Task<bool> DeleteCartItem(int CartItemID)
        {
            bool flag = await _cartItemData.DeleteCartItem(CartItemID);
            return flag;
        }
    }
}