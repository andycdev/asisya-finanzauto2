using Microsoft.AspNetCore.Mvc;
using asisya_api.Data;
using asisya_api.Models;
using asisya_api.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace asisya_api.Controllers;

[ApiController]
[Route("api/products")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(
       int page = 1,
       int pageSize = 20,
       string? search = null,
       int? categoryId = null)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.Name.Contains(search));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId);
        }

        var total = await query.CountAsync();

        var products = await query
     .OrderBy(p => p.Id) // importante para paginación
     .Skip((page - 1) * pageSize)
     .Take(pageSize)
     .Select(p => new ProductResponseDto
     {
         Id = p.Id,
         Name = p.Name,
         Description = p.Description,        // <<<<< agregado
         Price = p.Price,
         UnitsInStock = p.UnitsInStock,      // <<<<< agregado
         Discontinued = p.Discontinued,      // <<<<< agregado
         Category = new CategoryDto
         {
             Id = p.Category.Id,
             Name = p.Category.Name,
             PhotoUrl = p.Category.PhotoUrl
         }
     })
     .ToListAsync();


        var totalPages = (int)Math.Ceiling((double)total / pageSize);

        return Ok(new
        {
            total,
            totalPages,  // <<<<< agregado
            page,
            pageSize,
            data = products
        });

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.Id == id)
            .Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                UnitsInStock = p.UnitsInStock,
                Discontinued = p.Discontinued,
                Category = new CategoryDto
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name,
                    PhotoUrl = p.Category.PhotoUrl
                }
            })
            .SingleOrDefaultAsync(); // <- devuelve 1 producto o null

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto updated)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        product.Name = updated.Name;
        product.Description = updated.Description;
        product.Price = updated.Price;
        product.UnitsInStock = updated.UnitsInStock;
        product.Discontinued = updated.Discontinued;
        product.CategoryId = updated.CategoryId;

        await _context.SaveChangesAsync();
        return Ok(product);
    }



    [HttpPost]
    public async Task<IActionResult> GenerateProducts(GenerateProductsDto dto)
    {
        var categories = await _context.Categories.ToListAsync();

        if (!categories.Any())
            return BadRequest("No hay categorías creadas.");

        var random = new Random();

        var products = new List<Product>();

        for (int i = 0; i < dto.Count; i++)
        {
            var category = categories[random.Next(categories.Count)];

            products.Add(new Product
            {
                Name = $"Producto {Guid.NewGuid().ToString().Substring(0, 8)}",
                Price = random.Next(50, 5000),
                CategoryId = category.Id
            });
        }

        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        return Ok(new { inserted = products.Count });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateProduct(CreateProductDto dto)
    {
        var category = await _context.Categories.FindAsync(dto.CategoryId);
        if (category == null) return BadRequest("Categoría no válida");

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            UnitsInStock = dto.UnitsInStock,
            Discontinued = dto.Discontinued,
            CategoryId = dto.CategoryId
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Usar DTO para evitar ciclos
        var response = new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            UnitsInStock = product.UnitsInStock,
            Discontinued = product.Discontinued,
            Category = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                PhotoUrl = category.PhotoUrl
            }
        };

        return Ok(response);
    }

}