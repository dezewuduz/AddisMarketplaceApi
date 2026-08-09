using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AddisMarketplaceApi.Data;
using AddisMarketplaceApi.Models;

namespace AddisMarketplaceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/orders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        return await _context.Orders.ToListAsync();
    }

    // GET: api/orders/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return NotFound();
        return order;
    }

    // GET: api/orders/seller/1  ← አንድ ሻጭ የተቀበላቸው ትዕዛዞች ሁሉ
    [HttpGet("seller/{sellerId}")]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrdersBySeller(int sellerId)
    {
        return await _context.Orders
            .Where(o => o.Product!.SellerId == sellerId)
            .Include(o => o.Product)
            .ToListAsync();
    }

    // POST: api/orders
    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(Order order)
    {
        var product = await _context.Products.FindAsync(order.ProductId);
        if (product == null) return BadRequest("ProductId የተባለ ምርት አልተገኘም።");

        var buyerExists = await _context.Buyers.AnyAsync(b => b.Id == order.BuyerId);
        if (!buyerExists) return BadRequest("BuyerId የተባለ ገዥ አልተገኘም።");

        order.TotalPrice = product.Price * order.Quantity;
        order.Status = OrderStatus.Pending;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    // PUT: api/orders/5/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateStatusDto dto)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return NotFound();

        order.Status = dto.Status;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

public class UpdateStatusDto
{
    public OrderStatus Status { get; set; }
}