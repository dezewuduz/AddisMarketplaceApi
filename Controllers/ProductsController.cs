using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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

    // GET: api/products  ← ክፍት (ማንም ማየት ይችላል፣ ገዥዎች ምዝገባ አያስፈልጋቸውም)
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

    // GET: api/products/5  ← ክፍት
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

    // GET: api/products/seller/1  ← ክፍት
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

    // POST: api/products  ← ተጠብቋል፣ SellerId ራሱ ከ token ይመጣል
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ProductResponseDto>> CreateProduct(CreateProductDto dto)
    {
        var sellerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var seller = await _context.Sellers.FindAsync(sellerId);
        if (seller == null) return BadRequest("ሻጭ አልተገኘም።");

        var product = new Product
        {
            SellerId = sellerId,
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

    // PUT: api/products/5  ← ተጠብቋል፣ የራሱ ምርት ብቻ ማስተካከል ይችላል
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateProduct(int id, CreateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        var sellerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (product.SellerId != sellerId)
            return Forbid();   // 403 — ይሄ ምርት የአንተ አይደለም

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.PhotoUrl = dto.PhotoUrl;
        product.Category = dto.Category;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/products/5  ← ተጠብቋል፣ የራሱ ምርት ብቻ መሰረዝ ይችላል
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        var sellerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (product.SellerId != sellerId)
            return Forbid();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}