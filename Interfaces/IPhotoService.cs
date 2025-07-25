using System;
using PropertyListingAPI.Helpers;

namespace PropertyListingAPI.Interfaces;

public interface IPhotoService
{
    Task<PhotoUploadResult> UploadImageAsync(IFormFile file);
    Task DeleteImageAsync(string publicId);

}

