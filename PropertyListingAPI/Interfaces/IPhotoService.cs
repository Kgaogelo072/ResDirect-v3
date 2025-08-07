using System;
using PropertyListingAPI.Helpers;

namespace PropertyListingAPI.Interfaces;

public interface IPhotoService
{
    Task<PhotoUploadResult> UploadImageAsync(IFormFile file);
    Task<List<PhotoUploadResult>> UploadMultipleImagesAsync(List<IFormFile> files);
    Task DeleteImageAsync(string publicId);
    Task DeleteMultipleImagesAsync(List<string> publicIds);
}

