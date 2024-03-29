using Microsoft.EntityFrameworkCore;
using TerribleBankInc.Data.EntityConfigs;
using TerribleBankInc.Models.Entities;

namespace TerribleBankInc.Data
{
    public class TerribleBankDbContext : DbContext
    {
        public TerribleBankDbContext(DbContextOptions<TerribleBankDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BankTransaction> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new BankAccountEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BankTransactionEntityTypeConfiguration());
        }
    }
}