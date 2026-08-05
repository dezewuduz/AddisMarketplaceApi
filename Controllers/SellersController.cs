using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AddisMarketplaceApi.Data;
using AddisMarketplaceApi.Models;

namespace AddisMarketplaceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SellersController : ControllerBase
{
    private readonly AppDbContext _context;

    public SellersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/sellers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Seller>>> GetSellers()
    {
        return await _context.Sellers.ToListAsync();
    }

    // GET: api/sellers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Seller>> GetSeller(int id)
    {
        var seller = await _context.Sellers.FindAsync(id);
        if (seller == null) return NotFound();
        return seller;
    }

    // POST: api/sellers
    [HttpPost]
    public async Task<ActionResult<Seller>> CreateSeller(Seller seller)
    {
        _context.Sellers.Add(seller);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSeller), new { id = seller.Id }, seller);
    }

    // PUT: api/sellers/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSeller(int id, Seller seller)
    {
        if (id != seller.Id) return BadRequest();
        _context.Entry(seller).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/sellers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSeller(int id)
    {
        var seller = await _context.Sellers.FindAsync(id);
        if (seller == null) return NotFound();
        _context.Sellers.Remove(seller);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}