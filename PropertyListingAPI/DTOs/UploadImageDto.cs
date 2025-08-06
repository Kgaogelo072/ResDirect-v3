using System;

namespace PropertyListingAPI.DTOs;

public class UploadImageDto
{
    public required IFormFile File { get; set; }
}

