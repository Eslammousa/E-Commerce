using Microsoft.AspNetCore.Http;

namespace E_Commerce.Core.ServicesContracts
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile image, string folderName);

        Task DeleteImageAsync(string imagePath);

         Task<string> UpdateImageAsync(IFormFile newImage, string? oldImagePath, string folderName);

    }
}
