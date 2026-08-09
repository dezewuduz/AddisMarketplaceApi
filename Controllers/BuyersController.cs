using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AddisMarketplaceApi.Data;
using AddisMarketplaceApi.Models;

namespace AddisMarketplaceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BuyersController : ControllerBase
{
    private readonly AppDbContext _context;

    public BuyersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/buyers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Buyer>>> GetBuyers()
    {
        return await _context.Buyers.ToListAsync();
    }

    // GET: api/buyers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Buyer>> GetBuyer(int id)
    {
        var buyer = await _context.Buyers.FindAsync(id);
        if (buyer == null) return NotFound();
        return buyer;
    }

    // POST: api/buyers
    [HttpPost]
    public async Task<ActionResult<Buyer>> CreateBuyer(Buyer buyer)
    {
        _context.Buyers.Add(buyer);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBuyer), new { id = buyer.Id }, buyer);
    }
}