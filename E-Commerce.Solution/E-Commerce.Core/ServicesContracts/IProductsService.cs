using E_Commerce.Core.DTO.ProductDTO;

namespace E_Commerce.Core.ServicesContracts
{
    public interface IProductsService
    {
        Task<ProductResponse> AddProduct(ProductAddRequest productAddRequest);
        Task<ProductResponse> UpdateProduct(Guid Id ,ProudctUpdateRequest proudctUpdateRequest);
        Task<bool> DeleteProduct(Guid Id);


        Task<ProductResponse> GetProductByProductId(Guid id);
        Task<ProductResponse> GetProductByProductName(string name);


        Task<IEnumerable<ProductResponse>> GetAllProudcts();
        Task<IEnumerable<ProductResponse>> GetAllProudctsByCategoryId(Guid categoryId);
        Task<IEnumerable<ProductResponse>> Search(string keyword);

    }
}
