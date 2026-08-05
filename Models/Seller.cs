using System.ComponentModel.DataAnnotations;

namespace AddisMarketplaceApi.Models;

public class Seller
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Location { get; set; } = string.Empty;   // ለምሳሌ "መርካቶ", "ቦሌ"

    [Required, MaxLength(20)]
    [RegularExpression(@"^(09|07)\d{8}$", ErrorMessage = "ስልክ ቁጥር በ09 ወይም 07 መጀመር እና 10 ዲጂት መሆን አለበት")]
    public string PhoneNumber { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public bool IsVerified { get; set; } = false;

    public bool IsSubscribed { get; set; } = false;        // ወርሃዊ ምዝገባ ከፋይ ነው/አይደለም

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property — አንድ ሻጭ ብዙ ምርቶች ይኖሩታል
    public ICollection<Product> Products { get; set; } = new List<Product>();
}