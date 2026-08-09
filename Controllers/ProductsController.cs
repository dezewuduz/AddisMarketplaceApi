using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AddisMarketplaceApi.Data;
using AddisMarketplaceApi.Models;
using AddisMarketplaceApi.DTOs;

namespace AddisMarketplaceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts()
    {
        return await _context.Products
            .Include(p => p.Seller)
            .Select(p => new ProductResponseDto
            {
                Id = p.Id,
                SellerId = p.SellerId,
                SellerName = p.Seller!.Name,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                PhotoUrl = p.PhotoUrl,
                Category = p.Category,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();
    }

    // GET: api/products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponseDto>> GetProduct(int id)
    {
        var product = await _context.Products
            .Include(p => p.Seller)
            .Where(p => p.Id == id)
            .Select(p => new ProductResponseDto
            {
                Id = p.Id,
                SellerId = p.SellerId,
                SellerName = p.Seller!.Name,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                PhotoUrl = p.PhotoUrl,
                Category = p.Category,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (product == null) return NotFound();
        return product;
    }

    // GET: api/products/seller/1
    [HttpGet("seller/{sellerId}")]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProductsBySeller(int sellerId)
    {
        return await _context.Products
            .Include(p => p.Seller)
            .Where(p => p.SellerId == sellerId)
            .Select(p => new ProductResponseDto
            {
                Id = p.Id,
                SellerId = p.SellerId,
                SellerName = p.Seller!.Name,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                PhotoUrl = p.PhotoUrl,
                Category = p.Category,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> CreateProduct(CreateProductDto dto)
    {
        var seller = await _context.Sellers.FindAsync(dto.SellerId);
        if (seller == null) return BadRequest("SellerId የተባለ ሻጭ አልተገኘም።");

        var product = new Product
        {
            SellerId = dto.SellerId,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            PhotoUrl = dto.PhotoUrl,
            Category = dto.Category,
            IsActive = true
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var response = new ProductResponseDto
        {
            Id = product.Id,
            SellerId = product.SellerId,
            SellerName = seller.Name,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            PhotoUrl = product.PhotoUrl,
            Category = product.Category,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt
        };

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, response);
    }

    // PUT: api/products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, CreateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.PhotoUrl = dto.PhotoUrl;
        product.Category = dto.Category;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}