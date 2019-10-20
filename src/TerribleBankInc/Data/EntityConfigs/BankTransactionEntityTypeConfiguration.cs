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

            builder.HasOne(x => x.SourceAccount).WithMany(x => x.OutgoingTransactions).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.DestinationAccount).WithMany(x => x.IncomingTransactions).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.SourceClient);
            builder.HasOne(x => x.DestinationClient);
        }
    }
}