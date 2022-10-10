using Hubtel.Wallets.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Hubtel.Wallets.Api.Data
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Product>()
            //    .HasOne(c => c.Category)
            //    .WithMany(c => c.Products)
            //    .HasForeignKey(c => c.CategoryId);

            

        }
    }
}
