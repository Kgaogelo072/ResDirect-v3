using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using PropertyListingAPI.Helpers;
using PropertyListingAPI.Interfaces;

namespace PropertyListingAPI.Services;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;

    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(acc);
    }

    public async Task<PhotoUploadResult> UploadImageAsync(IFormFile file)
    {
        if (file.Length == 0) return new PhotoUploadResult();

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Height(500).Width(800).Crop("fill")
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error != null) throw new Exception(result.Error.Message);

        return new PhotoUploadResult
        {
            Url = result.SecureUrl.ToString(),
            PublicId = result.PublicId
        };
    }

    public async Task<List<PhotoUploadResult>> UploadMultipleImagesAsync(List<IFormFile> files)
    {
        var results = new List<PhotoUploadResult>();
        
        // Limit to maximum 5 images
        var filesToProcess = files.Take(5).Where(f => f.Length > 0).ToList();
        
        foreach (var file in filesToProcess)
        {
            try
            {
                var result = await UploadImageAsync(file);
                if (!string.IsNullOrEmpty(result.Url))
                {
                    results.Add(result);
                }
            }
            catch (Exception ex)
            {
                // Log error but continue with other images
                // In production, you might want to use a proper logging framework
                Console.WriteLine($"Failed to upload image {file.FileName}: {ex.Message}");
            }
        }
        
        return results;
    }

    public async Task DeleteImageAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);
        if (result.Result != "ok" && result.Result != "not found")
            throw new Exception($"Failed to delete image: {result.Error?.Message}");
    }

    public async Task DeleteMultipleImagesAsync(List<string> publicIds)
    {
        foreach (var publicId in publicIds.Where(id => !string.IsNullOrEmpty(id)))
        {
            try
            {
                await DeleteImageAsync(publicId);
            }
            catch (Exception ex)
            {
                // Log error but continue with other deletions
                Console.WriteLine($"Failed to delete image {publicId}: {ex.Message}");
            }
        }
    }
}

