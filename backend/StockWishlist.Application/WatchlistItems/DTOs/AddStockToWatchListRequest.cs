namespace StockWishlist.Application.WatchlistItems.DTOs;


public class AddStockToWatchListRequest
{
    public Guid WatchlistId {get;set;}
    public Guid StockId {get;set;}

    public decimal? TargetPrice {get;set;}
    public decimal? BuyPrice {get;set;}
}