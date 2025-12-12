using StockWishlist.Application.WatchlistItems.DTOs;
using StockWishlist.Domain.Entities;

namespace StockWishlist.Application.WatchlistItems.Interfaces;

public interface IWatchListItemService
{
    Task<WatchlistItem> AddStockToWatchListAsync(AddStockToWatchListRequest request);
}