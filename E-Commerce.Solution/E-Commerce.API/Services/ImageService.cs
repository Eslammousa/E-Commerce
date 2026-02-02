using E_Commerce.Core.Exceptions;
using E_Commerce.Core.ServicesContracts;

namespace E_Commerce.API.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;

        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveProductImageAsync(IFormFile image)
        {
            if (image == null || image.Length == 0)
                throw new InvalidImageTypeException("Image is required");

            if (string.IsNullOrWhiteSpace(_env.WebRootPath))
                throw new Exception("WebRootPath is not configured");

            var folderPath = Path.Combine(_env.WebRootPath, "images", "products");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            var allowed = new[] { ".jpg", ".jpeg", ".png" };

            if (!allowed.Contains(extension))
                throw new InvalidImageTypeException("Invalid image type");

            if (image.Length > 2 * 1024 * 1024)
                throw new ImageSizeExceededException("Image size must be less than 2MB");

            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.CreateNew);
            await image.CopyToAsync(stream);

            return $"images/products/{fileName}";
        }

        public async Task DeleteProductImageAsync(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return;

            if (string.IsNullOrWhiteSpace(_env.WebRootPath))
                throw new Exception("WebRootPath is not configured");

            var sanitizedPath = imagePath.Replace("/", Path.DirectorySeparatorChar.ToString());
            var fullPath = Path.Combine(_env.WebRootPath, sanitizedPath);

            var root = Path.GetFullPath(_env.WebRootPath);
            var file = Path.GetFullPath(fullPath);

            if (!file.StartsWith(root))
                throw new UnauthorizedAccessException("Invalid image path");

            if (File.Exists(file))
                await Task.Run(() => File.Delete(file));
        }

        public async Task<string> UpdateProductImageAsync(IFormFile newImage, string? oldImagePath)
        {
            if (newImage == null || newImage.Length == 0)
                throw new InvalidImageTypeException("Image is required");

            if (!string.IsNullOrWhiteSpace(oldImagePath))
                await DeleteProductImageAsync(oldImagePath);

            var newImagePath = await SaveProductImageAsync(newImage);

            return newImagePath;
        }



    }
}
