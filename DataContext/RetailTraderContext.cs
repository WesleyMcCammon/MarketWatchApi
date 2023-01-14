using Microsoft.EntityFrameworkCore;
namespace MarketWatch.DataContext;

internal class RetailTraderContext : DbContext
{
    public RetailTraderContext() : base(GetOptions())
    {
    }

    public virtual DbSet<Instrument> Instrument { get; set; } = default!;
    public virtual DbSet<Market> Market { get; set; } = default!;
    public virtual DbSet<MarketSector> MarketSector { get; set; } = default!;
    public virtual DbSet<InstrumentDataSource> InstrumentDataSource { get; set; } = default!;
    public virtual DbSet<CurrencyDescription> CurrencyDescriptions { get; set; } = default!;

    private static DbContextOptions GetOptions()
    {
        return SqlServerDbContextOptionsExtensions
            .UseSqlServer(new DbContextOptionsBuilder(), @"Server=localhost;Database=RetailTrader;Trusted_Connection=True;trustServerCertificate=true").Options;
    }
}