using Microsoft.AspNetCore.Mvc;
using asisya_api.Data;
using asisya_api.Models;
using asisya_api.DTOs;

namespace asisya_api.Controllers;

[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
   public async Task<IActionResult> Create(CategoryDto dto)
{
    var category = new Category
    {
        Name = dto.Name,
        PhotoUrl = dto.PhotoUrl
    };

    _context.Categories.Add(category);
    await _context.SaveChangesAsync();

    return Ok(category);
}

    [HttpGet]
    public IActionResult GetAll()
    {
        var categories = _context.Categories.ToList();

        return Ok(categories);
    }
}