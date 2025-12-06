using Microsoft.EntityFrameworkCore;
using StockWishlist.Domain.Entities;

namespace StockWishlist.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        :base(options)
    {
    }

    // Dbset tables

    public DbSet<User> Users => Set<User>();
    public DbSet<Watchlist> Watchlists => Set<Watchlist>();

    public DbSet<WatchlistItem> WatchlistItems => Set<WatchlistItem>();

    public DbSet<Stock> Stocks => Set<Stock>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //User - watchlist - 1 - many
        modelBuilder.Entity<User>()
            .HasMany(u => u.Watchlists)
            .WithOne(w => w.User)
            .HasForeignKey(w => w.UserId);

        //watchlist - watchlistitem - 1 - many
        modelBuilder.Entity<Watchlist>()
            .HasMany(w => w.Items)
            .WithOne(i => i.Watchlist)
            .HasForeignKey(i => i.WatchlistId);

        //Stock 1 - many 
        modelBuilder.Entity<Stock>()
            .HasMany(s => s.WatchlistItems)
            .WithOne(i => i.Stock)
            .HasForeignKey(i => i.StockId);

        //Make stock symbol unique
        modelBuilder.Entity<Stock>()
            .HasIndex(s => s.Symbol)
            .IsUnique();
    }
}