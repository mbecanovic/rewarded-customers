using Microsoft.EntityFrameworkCore;

namespace comtrade.RewardedCustomer
{
    public class RewardedCustomerContext : DbContext
    {
        public DbSet<RewardedCustomers> RewardedCustomers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ApiUsage> ApiUsages { get; set; }

        public RewardedCustomerContext(DbContextOptions<RewardedCustomerContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<Address>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<RewardedCustomers>()
                .HasOne(rc => rc.HomeAddress)
                .WithMany()
                .HasForeignKey(rc => rc.HomeAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RewardedCustomers>()
                .HasOne(rc => rc.OfficeAddress)
                .WithMany()
                .HasForeignKey(rc => rc.OfficeAddressId)
                .OnDelete(DeleteBehavior.Restrict);
        }



    }
}