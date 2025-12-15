using Microsoft.EntityFrameworkCore;
using StockWishlist.Application.Stocks;
using StockWishlist.Application.Watchlists.DTOs;
using StockWishlist.Application.Watchlists.Interfaces;
using StockWishlist.Domain.Entities;
using StockWishlist.Infrastructure.Data;
using StockWishlist.Infrastructure.Stocks;

namespace StockWishlist.Infrastructure.Watchlists;

public class WatchlistService : IWatchlistService
{
    private readonly AppDbContext _context;

    private readonly IStockPriceService _stockPriceService;

    public WatchlistService(AppDbContext context, IStockPriceService stockPriceService)
    {
        _context = context;
        _stockPriceService = stockPriceService;
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

        var dto = new WatchlistDetailsDto
        {
            WatchlistId = watchList.Id,
            Name = watchList.Name,
            Stocks = new List<WatchlistStockDto>()
        };

        foreach(var item in watchList.Items)
        {
            var livePrice = await _stockPriceService.GetLatestPriceAsync(item.Stock.Symbol);

            var stockDto = new WatchlistStockDto
            {
                StockId = item.Stock.Id,
                Symbol = item.Stock.Symbol,
                Name = item.Stock.Name,
                Sector = item.Stock.Sector,
                TargetPrice = item.TargetSellPrice,
                BuyPrice = item.TargetBuyPrice,
                LivePrice = livePrice
            };
            dto.Stocks.Add(stockDto);
        }

        return dto;
    }

}