namespace StockWishlist.Application.Watchlists.DTOs;

public class CreateWatchlistRequest
{
    public Guid UserId {get;set;}
    public string Name {get;set;} = string.Empty;
}