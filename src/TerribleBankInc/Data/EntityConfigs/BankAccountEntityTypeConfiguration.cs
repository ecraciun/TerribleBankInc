using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerribleBankInc.Models;
using TerribleBankInc.Models.Entities;

namespace TerribleBankInc.Data.EntityConfigs
{
    public class BankAccountEntityTypeConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.Property(x => x.ClientId).IsRequired();
            builder.Property(x => x.AccountNumber).IsRequired();
            builder.Property(x => x.Balance)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder
                .HasOne(x => x.Client)
                .WithMany(x => x.BankAccounts)
                .HasForeignKey(x => x.ClientId);
        }
    }
}