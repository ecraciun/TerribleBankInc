using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerribleBankInc.Models;
using TerribleBankInc.Models.Entities;

namespace TerribleBankInc.Data.EntityConfigs
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Username).IsRequired();
            builder.Property(x => x.ClientId).IsRequired();
            builder.Property(x => x.HashedPassword).IsRequired();

            builder
                .HasOne(x => x.Client)
                .WithOne(x => x.User);
        }
    }
}