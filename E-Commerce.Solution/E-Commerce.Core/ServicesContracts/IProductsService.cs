using E_Commerce.Core.Common;
using E_Commerce.Core.DTO;
using E_Commerce.Core.DTO.ProductDTO;

namespace E_Commerce.Core.ServicesContracts
{
    public interface IProductsService
    {
        Task<ProductResponse> AddProduct(ProductAddRequest productAddRequest);
        Task<ProductResponse> UpdateProduct(Guid Id ,ProudctUpdateRequest proudctUpdateRequest);
        Task<bool> DeleteProduct(Guid Id);
        Task<bool> RestoreProduct(Guid Id);
        Task<PagedResult<ProductResponse>> GetDeletedProducts(PaginationDTO paginationDTO);

        Task<ResponseProductWithReview> GetProductByProductId(Guid id);
        Task<ProductResponse> GetProductByProductName(string name);


        Task<PagedResult<ProductResponse>> GetAllProducts(PaginationDTO paginationDTO);
        Task<PagedResult<ProductResponse>> GetAllProudctsByCategoryId(Guid categoryId , PaginationDTO paginationDTO);
        Task<PagedResult<ProductResponse>> Search(string keyword, PaginationDTO paginationDTO);

    }
}
