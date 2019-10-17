using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerribleBankInc.Models;
using TerribleBankInc.Models.Entities;

namespace TerribleBankInc.Data.EntityConfigs
{
    public class BankTransactionEntityTypeConfiguration : IEntityTypeConfiguration<BankTransaction>
    {
        public void Configure(EntityTypeBuilder<BankTransaction> builder)
        {
            builder.Property(x => x.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.Property(x => x.DestinationClientEmail).IsRequired();

            builder.HasOne(x => x.SourceBankAccount).WithMany(x => x.OutgoingTransactions).HasForeignKey(x => x.SourceAccountId);
            builder.HasOne(x => x.DestinationBankAccount).WithMany(x => x.IncomingTransactions).HasForeignKey(x => x.DestinationAccountId);
            builder.HasOne(x => x.SourceClient);
            builder.HasOne(x => x.DestinationClient);
        }
    }
}