using System.ComponentModel.DataAnnotations;

namespace AddisMarketplaceApi.DTOs;

// POST/PUT ላይ ገዥ (frontend) የሚልከው shape
public class CreateProductDto
{
    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, 1000000)]
    public decimal Price { get; set; }

    [MaxLength(500)]
    public string? PhotoUrl { get; set; }

    [Required, MaxLength(100)]
    public string Category { get; set; } = string.Empty;
}

// GET ላይ የሚመለስ shape
public class ProductResponseDto
{
    public int Id { get; set; }
    public int SellerId { get; set; }
    public string SellerName { get; set; } = string.Empty;   // ← Seller ሙሉ object ፈንታ ስም ብቻ
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? PhotoUrl { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}