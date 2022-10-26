using CashPoint.Models;
using Microsoft.EntityFrameworkCore;

namespace CashPoint.EntityFramework
{
    public class CashPointContext :DbContext
    {
        public CashPointContext(DbContextOptions<CashPointContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
        }
    }
}
