using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using StockWishlist.Application.WatchlistItems.DTOs;
using StockWishlist.Application.WatchlistItems.Interfaces;
using StockWishlist.Application.Watchlists.DTOs;
using StockWishlist.Domain.Entities;
using StockWishlist.Infrastructure.Data;

namespace StockWishlist.Infrastructure.WatchlistItems;


public class WatchListItemService : IWatchListItemService
{
    private readonly AppDbContext _context;
    public WatchListItemService(AppDbContext context)
    {
        _context = context;
    }


    public async Task<WatchlistItem> AddStockToWatchListAsync(AddStockToWatchListRequest request)
    {
        var watchListExists = await _context.Watchlists
            .AnyAsync(w => w.Id == request.WatchlistId);

        if(!watchListExists)
            throw new Exception("Watchlist not found");


        var stockExists = await _context.Stocks
            .AnyAsync(s => s.Id == request.StockId);

        if(!stockExists)
            throw new Exception("Stock not found");


        var alreadyAdded = await _context.WatchlistItems
            .AnyAsync(wi => wi.WatchlistId == request.WatchlistId
                            && wi.StockId == request.StockId);


        if(alreadyAdded)
            throw new Exception("Stock already added to the watchlist");


        var item = new WatchlistItem
        {
            Id = Guid.NewGuid(),
            WatchlistId = request.WatchlistId,
            StockId = request.StockId,
            TargetBuyPrice = request.TargetPrice,
            TargetSellPrice = request.BuyPrice
        };

        _context.WatchlistItems.Add(item);
        await _context.SaveChangesAsync();

        return item;
    }
}