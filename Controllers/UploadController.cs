using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyListingAPI.DTOs;
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
    public async Task<IActionResult> UploadImage([FromForm] UploadImageDto dto)
    {
        var result = await _photoService.UploadImageAsync(dto.File);
        return Ok(result);
    }
}
