using StockWishlist.Application.Watchlists.DTOs;
using StockWishlist.Domain.Entities;

namespace StockWishlist.Application.Watchlists.Interfaces;


public interface IWatchlistService
{
    Task<Watchlist> CreateWatchlistAsync(CreateWatchlistRequest request);

    Task<WatchlistDetailsDto> GetWatchlistWithStocksAsync(Guid id);
}