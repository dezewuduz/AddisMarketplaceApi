using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddisMarketplaceApi.Models;

public enum OrderStatus
{
    Pending,      // ገዥ ትዕዛዝ ሰጥቷል
    Confirmed,    // ሻጭ አረጋግጧል
    Completed,    // ሽያጭ ተጠናቋል
    Cancelled     // ተሰርዟል
}

public class Order
{
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }

    [Required]
    public int BuyerId { get; set; }

    [ForeignKey(nameof(BuyerId))]
    public Buyer? Buyer { get; set; }

    [Range(1, 1000)]
    public int Quantity { get; set; } = 1;

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}