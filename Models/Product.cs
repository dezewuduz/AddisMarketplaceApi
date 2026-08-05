using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddisMarketplaceApi.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    public int SellerId { get; set; }

    [ForeignKey(nameof(SellerId))]
    public Seller? Seller { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, 1000000, ErrorMessage = "ዋጋ ከ0 በላይ መሆን አለበት")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [MaxLength(300)]
    public string? PhotoUrl { get; set; }

    [Required, MaxLength(50)]
    public string Category { get; set; } = string.Empty;   // ለምሳሌ "ልብስ", "ኤሌክትሮኒክስ"

    public bool IsActive { get; set; } = true;              // ሻጭ ምርቱን ደብቆ/አጥፍቶ ማድረግ ከፈለገ

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}