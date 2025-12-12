using Microsoft.EntityFrameworkCore;
using StockWishlist.Application.Watchlists.DTOs;
using StockWishlist.Application.Watchlists.Interfaces;
using StockWishlist.Domain.Entities;
using StockWishlist.Infrastructure.Data;

namespace StockWishlist.Infrastructure.Watchlists;

public class WatchlistService : IWatchlistService
{
    private readonly AppDbContext _context;

    public WatchlistService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Watchlist> CreateWatchlistAsync(CreateWatchlistRequest request)
    {
        var userExists = await _context.Users
            .AnyAsync(u => u.Id == request.UserId);

        if(!userExists)
        {
            throw new Exception("User not found");
        }

        var watchList = new Watchlist
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Watchlists.Add(watchList);
        await _context.SaveChangesAsync();

        return watchList;
    }

    public async Task<WatchlistDetailsDto?> GetWatchlistWithStocksAsync(Guid watchlistId)
    {
        var watchList = await _context.Watchlists
            .Include(w => w.Items)
                .ThenInclude(wi => wi.Stock)
            .FirstOrDefaultAsync(w => w.Id == watchlistId);

        if(watchList == null)
        {
            return null;
        }

        return new WatchlistDetailsDto
        {
            WatchlistId = watchList.Id,
            Name = watchList.Name,
            Stocks = watchList.Items.Select(wi => new WatchlistStockDto
            {
                StockId = wi.Stock.Id,
                Symbol = wi.Stock.Symbol,
                Name = wi.Stock.Name,
                Sector = wi.Stock.Sector,
                TargetPrice = wi.TargetSellPrice,
                BuyPrice = wi.TargetBuyPrice
            }).ToList()

        };
    }

}