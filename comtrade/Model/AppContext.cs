using Microsoft.EntityFrameworkCore;

namespace comtrade.Model
{
    public class ApplicationDbContext : DbContext //koristimo Nuget paket za set-ovanje DB-a
    {
        public DbSet<Customer> Customers { get; set; } //definisemo tabelu u bazi

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }
    }
}
