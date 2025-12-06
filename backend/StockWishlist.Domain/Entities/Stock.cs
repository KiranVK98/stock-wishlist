namespace StockWishlist.Domain.Entities;

public class Stock
{
    public Guid Id {get;set;}

    public string Symbol {get;set;} = null;
    public string Name {get;set;} = null;
    public string Sector {get;set;} = null;

    public ICollection<WatchlistItem> WatchlistItems {get;set;} = new List<WatchlistItem>();
}