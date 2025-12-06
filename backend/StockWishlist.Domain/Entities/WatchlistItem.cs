namespace StockWishlist.Domain.Entities;

public class WatchlistItem
{
    public Guid Id {get;set;}

    public Guid WatchlistId {get;set;}
    public Watchlist Watchlist {get;set;} = null;

    public Guid StockId {get;set;}
    public Stock Stock {get;set;} = null;

    public decimal? TargetBuyPrice {get;set;}
    public decimal? TargetSellPrice {get;set;}
    public string? Notes{get;set;}

    public DateTime AddedAt{get;set;} = DateTime.UtcNow;
}