using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PropertyListingAPI.Data;
using PropertyListingAPI.DTOs;
using PropertyListingAPI.Models;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PropertiesController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var properties = await _context.Properties.Include(p => p.Agent).ToListAsync();
        return Ok(_mapper.Map<IEnumerable<PropertyReadDto>>(properties));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var property = await _context.Properties.Include(p => p.Agent).FirstOrDefaultAsync(p => p.Id == id);
        if (property == null) return NotFound();

        return Ok(_mapper.Map<PropertyReadDto>(property));
    }

    [HttpPost]
    [Authorize(Roles = "Agent")]
    public async Task<IActionResult> Create(PropertyCreateDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var property = _mapper.Map<Property>(dto);
        property.AgentId = userId;

        _context.Properties.Add(property);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<PropertyReadDto>(property));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Agent")]
    public async Task<IActionResult> Update(int id, PropertyCreateDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var property = await _context.Properties.FindAsync(id);
        if (property == null) return NotFound();
        if (property.AgentId != userId) return Forbid();

        _mapper.Map(dto, property);
        await _context.SaveChangesAsync();

        return Ok("Property updated.");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Agent")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var property = await _context.Properties.FindAsync(id);
        if (property == null) return NotFound();
        if (property.AgentId != userId) return Forbid();

        _context.Properties.Remove(property);
        await _context.SaveChangesAsync();

        return Ok("Property deleted.");
    }
}
