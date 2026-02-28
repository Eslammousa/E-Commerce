using E_Commerce.Core.DTO.CartDTO;

namespace E_Commerce.Core.ServicesContracts
{
    public interface ICartService
    {
        Task<CartResponse> AddCart( CartAddRequest cartAddRequest);
        Task<bool> RemoveFromCart( Guid cartItemId);
        Task<CartResponse> GetCartByUserId();
        public Task<CartResponse> EditCartItem(Guid cartItemId, int quantity);
        public Task<bool> ClearCart();
    }
}
