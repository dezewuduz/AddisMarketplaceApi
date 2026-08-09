using System.ComponentModel.DataAnnotations;

namespace AddisMarketplaceApi.DTOs;

public class CreateSellerDto
{
    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Location { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    [RegularExpression(@"^(09|07)\d{8}$", ErrorMessage = "ስልክ ቁጥር በ09 ወይም 07 መጀመር እና 10 ዲጂት መሆን አለበት")]
    public string PhoneNumber { get; set; } = string.Empty;
}

// ለ public listing (ገዥዎች የሚያዩት) — ስልክ ቁጥር የለም
public class SellerResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
    public bool IsSubscribed { get; set; }
    public int ProductCount { get; set; }
}
