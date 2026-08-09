using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AddisMarketplaceApi.Data;
using AddisMarketplaceApi.Models;
using AddisMarketplaceApi.DTOs;

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
    public async Task<ActionResult<IEnumerable<SellerResponseDto>>> GetSellers()
    {
        return await _context.Sellers
            .Select(s => new SellerResponseDto
            {
                Id = s.Id,
                Name = s.Name,
                Location = s.Location,
                IsVerified = s.IsVerified,
                IsSubscribed = s.IsSubscribed,
                ProductCount = s.Products.Count
            })
            .ToListAsync();
    }

    // GET: api/sellers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SellerResponseDto>> GetSeller(int id)
    {
        var seller = await _context.Sellers
            .Where(s => s.Id == id)
            .Select(s => new SellerResponseDto
            {
                Id = s.Id,
                Name = s.Name,
                Location = s.Location,
                IsVerified = s.IsVerified,
                IsSubscribed = s.IsSubscribed,
                ProductCount = s.Products.Count
            })
            .FirstOrDefaultAsync();

        if (seller == null) return NotFound();
        return seller;
    }

    // POST: api/sellers
    [HttpPost]
    public async Task<ActionResult<SellerResponseDto>> CreateSeller(CreateSellerDto dto)
    {
        var seller = new Seller
        {
            Name = dto.Name,
            Location = dto.Location,
            PhoneNumber = dto.PhoneNumber,
            IsVerified = false,
            IsSubscribed = false
        };

        _context.Sellers.Add(seller);
        await _context.SaveChangesAsync();

        var response = new SellerResponseDto
        {
            Id = seller.Id,
            Name = seller.Name,
            Location = seller.Location,
            IsVerified = seller.IsVerified,
            IsSubscribed = seller.IsSubscribed,
            ProductCount = 0
        };

        return CreatedAtAction(nameof(GetSeller), new { id = seller.Id }, response);
    }

    // PUT: api/sellers/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSeller(int id, CreateSellerDto dto)
    {
        var seller = await _context.Sellers.FindAsync(id);
        if (seller == null) return NotFound();

        seller.Name = dto.Name;
        seller.Location = dto.Location;
        seller.PhoneNumber = dto.PhoneNumber;

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
    // GET: api/sellers/5/contact
[HttpGet("{id}/contact")]
public async Task<ActionResult<object>> GetSellerContact(int id)
{
    var seller = await _context.Sellers.FindAsync(id);
    if (seller == null) return NotFound();
    return new { phoneNumber = seller.PhoneNumber };
}
}