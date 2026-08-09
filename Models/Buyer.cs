using System.ComponentModel.DataAnnotations;

namespace AddisMarketplaceApi.Models;

public class Buyer
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    [RegularExpression(@"^(09|07)\d{8}$", ErrorMessage = "ስልክ ቁጥር በ09 ወይም 07 መጀመር እና 10 ዲጂት መሆን አለበት")]
    public string PhoneNumber { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property — አንድ ገዥ ብዙ ትዕዛዞች ይኖሩታል
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}