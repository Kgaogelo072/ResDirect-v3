using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using PropertyListingAPI.Helpers;
using PropertyListingAPI.Interfaces;

namespace PropertyListingAPI.Services;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;
    private readonly ILogger<PhotoService> _logger;

    public PhotoService(IOptions<CloudinarySettings> config, ILogger<PhotoService> logger)
    {
        _logger = logger;
        
        var account = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );
        
        _cloudinary = new Cloudinary(account);
        
        // Log configuration (without sensitive data)
        _logger.LogInformation($"Cloudinary initialized with CloudName: {config.Value.CloudName}");
    }

    public async Task<PhotoUploadResult> UploadImageAsync(IFormFile file)
    {
        _logger.LogInformation($"Attempting to upload image: {file.FileName}, Size: {file.Length} bytes");
        
        using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Height(500).Width(800).Crop("fill")
        };

        try
        {
            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
            {
                _logger.LogError($"Cloudinary upload error: {result.Error.Message}");
                throw new Exception(result.Error.Message);
            }

            _logger.LogInformation($"Image uploaded successfully: {result.SecureUrl}");
            
            return new PhotoUploadResult
            {
                Url = result.SecureUrl.ToString(),
                PublicId = result.PublicId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception during image upload: {ex.Message}");
            throw;
        }
    }

    public async Task<List<PhotoUploadResult>> UploadMultipleImagesAsync(List<IFormFile> files)
    {
        _logger.LogInformation($"Attempting to upload {files.Count} images");
        
        var results = new List<PhotoUploadResult>();
        
        // Limit to maximum 5 images
        var filesToProcess = files.Take(5).Where(f => f.Length > 0).ToList();
        _logger.LogInformation($"Processing {filesToProcess.Count} valid files");
        
        foreach (var file in filesToProcess)
        {
            try
            {
                var result = await UploadImageAsync(file);
                if (!string.IsNullOrEmpty(result.Url))
                {
                    results.Add(result);
                    _logger.LogInformation($"Successfully uploaded image {file.FileName}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to upload image {file.FileName}: {ex.Message}");
                // Continue with other images but log the error
            }
        }
        
        _logger.LogInformation($"Upload completed. Successfully uploaded {results.Count} out of {filesToProcess.Count} images");
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
                _logger.LogError($"Failed to delete image {publicId}: {ex.Message}");
            }
        }
    }
}

