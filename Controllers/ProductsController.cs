using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using AddisMarketplaceApi.Data;
using AddisMarketplaceApi.Models;

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
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _context.Products.Include(p => p.Seller).ToListAsync();
    }

    // GET: api/products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.Include(p => p.Seller)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return NotFound();
        return product;
    }

    // GET: api/products/seller/1
    [HttpGet("seller/{sellerId}")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsBySeller(int sellerId)
    {
        return await _context.Products.Where(p => p.SellerId == sellerId).ToListAsync();
    }

    // POST: api/products
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        var sellerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        product.SellerId = sellerId;

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    // PUT: api/products/5
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id) return BadRequest();

        var existingProduct = await _context.Products.FindAsync(id);
        if (existingProduct == null) return NotFound();

        var sellerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (existingProduct.SellerId != sellerId)
            return Forbid();

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.PhotoUrl = product.PhotoUrl;
        existingProduct.Category = product.Category;
        existingProduct.IsActive = product.IsActive;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/products/5
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