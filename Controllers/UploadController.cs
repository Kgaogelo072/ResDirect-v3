using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyListingAPI.Interfaces;

namespace PropertyListingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Agent")]
public class UploadController : ControllerBase
{
    private readonly IPhotoService _photoService;

    public UploadController(IPhotoService photoService)
    {
        _photoService = photoService;
    }

    [HttpPost("property-image")]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
    {
        var imageUrl = await _photoService.UploadImageAsync(file);
        return Ok(new { imageUrl });
    }
}
