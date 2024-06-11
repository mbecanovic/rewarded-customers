using Microsoft.EntityFrameworkCore;

namespace comtrade.RewardedCustomer
{
    public class RewardedCustomerContext : DbContext
    {
        public DbSet<RewardedCustomer> RewardedCustomers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public RewardedCustomerContext(DbContextOptions<RewardedCustomerContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("rewardedCustomers");

            modelBuilder.Entity<Address>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<RewardedCustomer>()
                .HasOne(rc => rc.Home)
                .WithOne(a => a.RewardedCustomer)
                .HasForeignKey<Address>(a => a.HomeId);

            modelBuilder.Entity<RewardedCustomer>()
                .HasOne(rc => rc.Office)
                .WithOne(a => a.RewardedCustomer)
                .HasForeignKey<Address>(a => a.OfficeId);

            modelBuilder.Entity<RewardedCustomer>()
                .Ignore(rc => rc.HomeAddress);

            modelBuilder.Entity<RewardedCustomer>()
                .Ignore(rc => rc.OfficeAddress);
        }



    }
}