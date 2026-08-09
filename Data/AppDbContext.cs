using Microsoft.EntityFrameworkCore;
using AddisMarketplaceApi.Models;

namespace AddisMarketplaceApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Seller> Sellers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Buyer> Buyers { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Seller)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.SellerId)
            .OnDelete(DeleteBehavior.Cascade);   // ሻጭ ሲጠፋ ምርቶቹም አብረው ይጠፋሉ

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Product)
            .WithMany()
            .HasForeignKey(o => o.ProductId)
            .OnDelete(DeleteBehavior.Restrict);  // ትዕዛዝ ያለበት ምርት እንዳይሰረዝ ይከለክላል

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Buyer)
            .WithMany(b => b.Orders)
            .HasForeignKey(o => o.BuyerId)
            .OnDelete(DeleteBehavior.Cascade);   // ገዥ ሲጠፋ ትዕዛዞቹም አብረው ይጠፋሉ
    }
}