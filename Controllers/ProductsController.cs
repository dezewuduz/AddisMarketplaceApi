using Microsoft.AspNetCore.Mvc;
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

    // GET: api/products/seller/1  ← ለአንድ ሻጭ ምርቶች ብቻ
    [HttpGet("seller/{sellerId}")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsBySeller(int sellerId)
    {
        return await _context.Products.Where(p => p.SellerId == sellerId).ToListAsync();
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        var sellerExists = await _context.Sellers.AnyAsync(s => s.Id == product.SellerId);
        if (!sellerExists) return BadRequest("SellerId የተባለ ሻጭ አልተገኘም።");

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    // PUT: api/products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id) return BadRequest();
        _context.Entry(product).State = EntityState.Modified;
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