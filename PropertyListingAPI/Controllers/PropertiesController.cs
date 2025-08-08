using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PropertyListingAPI.Data;
using PropertyListingAPI.DTOs;
using PropertyListingAPI.Models;
using System.Security.Claims;
using PropertyListingAPI.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;
    private readonly ILogger<PropertiesController> _logger;

    public PropertiesController(ApplicationDbContext context, IMapper mapper, IPhotoService photoService, ILogger<PropertiesController> logger)
    {
        _context = context;
        _mapper = mapper;
        _photoService = photoService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var properties = await _context.Properties
            .Include(p => p.Agent)
            .Include(p => p.Images.OrderBy(i => i.DisplayOrder))
            .ToListAsync();
        return Ok(_mapper.Map<IEnumerable<PropertyReadDto>>(properties));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var property = await _context.Properties
            .Include(p => p.Agent)
            .Include(p => p.Images.OrderBy(i => i.DisplayOrder))
            .FirstOrDefaultAsync(p => p.Id == id);
        if (property == null) return NotFound();

        return Ok(_mapper.Map<PropertyReadDto>(property));
    }

    [HttpGet("by-agent")]
    [Authorize(Roles = "Agent")]
    public async Task<IActionResult> GetByAgent()
    {
        var agentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var properties = await _context.Properties
            .Include(p => p.Agent)
            .Include(p => p.Images.OrderBy(i => i.DisplayOrder))
            .Where(p => p.AgentId == agentId)
            .OrderByDescending(p => p.Id)
            .ToListAsync();
        
        return Ok(_mapper.Map<IEnumerable<PropertyReadDto>>(properties));
    }

    [HttpPost]
    [Authorize(Roles = "Agent")]
    public async Task<IActionResult> Create([FromForm] PropertyCreateDto dto)
    {
        // Validation
        if (dto.Images == null || dto.Images.Count == 0)
            return BadRequest("At least 1 image is required");

        if (dto.Images.Count > 5)
            return BadRequest("Maximum 5 images allowed");

        if (dto.PrimaryImageIndex < 0 || dto.PrimaryImageIndex >= dto.Images.Count)
            return BadRequest("Invalid primary image index");

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var property = _mapper.Map<Property>(dto);
        property.AgentId = userId;

        try
        {
            _logger.LogInformation($"Creating property with {dto.Images.Count} images for user {userId}");
            
            // Upload multiple images
            var uploadResults = await _photoService.UploadMultipleImagesAsync(dto.Images);
            
            if (uploadResults.Count == 0)
            {
                _logger.LogError("Failed to upload any images to Cloudinary");
                return BadRequest("Failed to upload any images. Please check your internet connection and try again.");
            }

            _logger.LogInformation($"Successfully uploaded {uploadResults.Count} images");

            // Create PropertyImage entities
            for (int i = 0; i < uploadResults.Count; i++)
            {
                var imageOrder = dto.ImageOrders.Count > i ? dto.ImageOrders[i] : i + 1;
                var isPrimary = i == dto.PrimaryImageIndex;

                property.Images.Add(new PropertyImage
                {
                    ImageUrl = uploadResults[i].Url,
                    ImagePublicId = uploadResults[i].PublicId,
                    DisplayOrder = imageOrder,
                    IsPrimary = isPrimary,
                    CreatedAt = DateTime.UtcNow
                });
            }

            _context.Properties.Add(property);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Property created successfully with ID: {property.Id}");

            // Reload property with images for response
            var createdProperty = await _context.Properties
                .Include(p => p.Agent)
                .Include(p => p.Images.OrderBy(i => i.DisplayOrder))
                .FirstOrDefaultAsync(p => p.Id == property.Id);

            return Ok(_mapper.Map<PropertyReadDto>(createdProperty));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating property: {ex.Message}");
            return BadRequest($"Failed to create property: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Agent")]
    public async Task<IActionResult> Update(int id, [FromForm] PropertyCreateDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var property = await _context.Properties
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (property == null) return NotFound();
        if (property.AgentId != userId) return Forbid();

        try
        {
            // If new images are provided, replace all existing images
            if (dto.Images != null && dto.Images.Count > 0)
            {
                if (dto.Images.Count > 5)
                    return BadRequest("Maximum 5 images allowed");

                if (dto.PrimaryImageIndex < 0 || dto.PrimaryImageIndex >= dto.Images.Count)
                    return BadRequest("Invalid primary image index");

                // Delete old images from Cloudinary
                var oldPublicIds = property.Images.Select(img => img.ImagePublicId).ToList();
                if (oldPublicIds.Any())
                {
                    await _photoService.DeleteMultipleImagesAsync(oldPublicIds);
                }

                // Remove old images from database
                _context.PropertyImages.RemoveRange(property.Images);

                // Upload new images
                var uploadResults = await _photoService.UploadMultipleImagesAsync(dto.Images);
                
                if (uploadResults.Count == 0)
                    return BadRequest("Failed to upload any images");

                // Create new PropertyImage entities
                property.Images.Clear();
                for (int i = 0; i < uploadResults.Count; i++)
                {
                    var imageOrder = dto.ImageOrders.Count > i ? dto.ImageOrders[i] : i + 1;
                    var isPrimary = i == dto.PrimaryImageIndex;

                    property.Images.Add(new PropertyImage
                    {
                        ImageUrl = uploadResults[i].Url,
                        ImagePublicId = uploadResults[i].PublicId,
                        DisplayOrder = imageOrder,
                        IsPrimary = isPrimary,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            // Update other property fields
            property.Title = dto.Title;
            property.Description = dto.Description;
            property.RentalAmount = dto.RentalAmount;
            property.Address = dto.Address;
            property.Bedrooms = dto.Bedrooms;
            property.Bathrooms = dto.Bathrooms;

            await _context.SaveChangesAsync();

            return Ok("Property updated successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to update property: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Agent")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var property = await _context.Properties
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (property == null) return NotFound();
        if (property.AgentId != userId) return Forbid();

        try
        {
            // Delete all images from Cloudinary
            var publicIds = property.Images.Select(img => img.ImagePublicId).ToList();
            if (publicIds.Any())
            {
                await _photoService.DeleteMultipleImagesAsync(publicIds);
            }

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();

            return Ok("Property deleted successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to delete property: {ex.Message}");
        }
    }
}
