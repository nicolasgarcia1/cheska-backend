using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services;

public interface ICloudinaryService
{
    Task<(string Url, string PublicId)> UploadImageAsync(IFormFile file, string folder = "perfumes");
    Task DeleteImageAsync(string publicId);
}