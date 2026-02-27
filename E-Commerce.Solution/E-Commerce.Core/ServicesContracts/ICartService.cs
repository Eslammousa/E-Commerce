using E_Commerce.Core.DTO.CartDTO;

namespace E_Commerce.Core.ServicesContracts
{
    public interface ICartService
    {
        Task<CartResponse> AddCart(Guid UserId, CartAddRequest cartAddRequest);
        Task<bool> RemoveFromCart(Guid userId, Guid cartItemId);
        Task<CartResponse> GetCartByUserId(Guid userId);
        public Task<CartResponse> EditCartItem(Guid userId, Guid cartItemId, int quantity);
        public Task<bool> ClearCart(Guid userId);
    }
}
