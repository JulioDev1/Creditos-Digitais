using Carteiras_Digitais.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Carteiras_Digitais.Infrasctruture.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<User> users { get; set; }
        public DbSet<Wallet> wallets { get; set; }
        public DbSet<Transaction> transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Wallet>()
                .HasOne(w => w.user)                
                .WithOne(u => u.wallet)           
                .HasForeignKey<Wallet>(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Wallet)
                .WithMany(w=> w.Transactions)
                .HasForeignKey(t => t.SenderWalletId)
                .IsRequired();
        }
    }
}
