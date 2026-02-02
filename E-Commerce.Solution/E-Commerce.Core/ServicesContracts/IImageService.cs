using Microsoft.AspNetCore.Http;

namespace E_Commerce.Core.ServicesContracts
{
    public interface IImageService
    {
        Task<string> SaveProductImageAsync(IFormFile image);

        Task DeleteProductImageAsync(string imagePath);

         Task<string> UpdateProductImageAsync(IFormFile newImage, string? oldImagePath);

    }
}
