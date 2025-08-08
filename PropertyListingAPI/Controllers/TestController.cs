using Microsoft.AspNetCore.Mvc;
using PropertyListingAPI.Interfaces;

namespace PropertyListingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IPhotoService _photoService;
    private readonly ILogger<TestController> _logger;

    public TestController(IPhotoService photoService, ILogger<TestController> logger)
    {
        _photoService = photoService;
        _logger = logger;
    }

    [HttpPost("upload-test")]
    public async Task<IActionResult> TestUpload([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided");

        try
        {
            _logger.LogInformation($"Testing upload of file: {file.FileName}, Size: {file.Length}");
            
            var result = await _photoService.UploadImageAsync(file);
            
            return Ok(new { 
                success = true, 
                url = result.Url, 
                publicId = result.PublicId,
                message = "Upload successful" 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Upload test failed: {ex.Message}");
            return BadRequest(new { 
                success = false, 
                error = ex.Message,
                message = "Upload failed" 
            });
        }
    }
} 