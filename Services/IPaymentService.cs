namespace AddisMarketplaceApi.Services;

// ወደፊት telebirr/CBE Birr እውነተኛ credentials ስናገኝ፣ ይህን implement የሚያደርግ class እንጨምራለን
public interface IPaymentService
{
    Task<bool> ProcessPaymentAsync(int orderId, decimal amount, string paymentMethod);
}

// ለአሁን — cash/manual ክፍያ ብቻ (ሻጭ በእጅ "ተከፍሏል" ብሎ ያረጋግጣል)
public class ManualPaymentService : IPaymentService
{
    public Task<bool> ProcessPaymentAsync(int orderId, decimal amount, string paymentMethod)
    {
        return Task.FromResult(true);
    }
}