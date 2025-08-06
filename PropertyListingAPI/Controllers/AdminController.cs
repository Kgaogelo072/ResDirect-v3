using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyListingAPI.Data;
using PropertyListingAPI.DTOs;

namespace PropertyListingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AdminController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/admin/unapproved-users
    [HttpGet("unapproved-users")]
    public async Task<IActionResult> GetUnapprovedUsers()
    {
        var users = await _context.Users
            .Where(u => !u.IsApproved)
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<UserReadDto>>(users));
    }

    // PUT: api/admin/approve/5
    [HttpPut("approve/{id}")]
    public async Task<IActionResult> ApproveUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        user.IsApproved = true;
        await _context.SaveChangesAsync();

        return Ok("User approved.");
    }

    // DELETE: api/admin/delete/5
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok("User deleted.");
    }
}

