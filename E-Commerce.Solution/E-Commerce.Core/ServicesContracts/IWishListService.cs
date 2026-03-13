using E_Commerce.Core.DTO.WishlistDTO;

namespace E_Commerce.Core.ServicesContracts
{
    public interface IWishListService
    {
        Task<WishlistResponse>AddProductInWishList(WishlistAddRequest request);
        Task<bool> DeleteProductFromWishList(WishlistAddRequest request);
         Task<IEnumerable<WishlistResponse>> GetProductsInWishList();
        Task<bool> ClearWishListAsync();

    }
}
