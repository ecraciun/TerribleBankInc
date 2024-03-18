using DefinitelyNotStealing.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DefinitelyNotStealing.Data;

public class AppDataContext : DbContext
{
    public AppDataContext(DbContextOptions options)
        : base(options) { }

    public DbSet<ExfiltratedData> Goodies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ExfiltratedData>().ToTable(nameof(Goodies));

        modelBuilder.Entity<ExfiltratedData>().Property(x => x.ID).IsRequired();
        modelBuilder.Entity<ExfiltratedData>().Property(x => x.Data).IsRequired();
        modelBuilder.Entity<ExfiltratedData>().Property(x => x.ClientIP).IsRequired();
        modelBuilder.Entity<ExfiltratedData>().Property(x => x.CorrelationId).IsRequired();
        modelBuilder.Entity<ExfiltratedData>().Property(x => x.DataType).IsRequired();
        modelBuilder.Entity<ExfiltratedData>().Property(x => x.Timestamp).IsRequired();
    }
}
