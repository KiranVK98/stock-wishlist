namespace StockWishlist.Domain.Entities;

public class Watchlist
{
    public Guid Id {get;set;}

    public string Name {get;set;} = null;

    public Guid UserId {get;set;}
    public User User {get;set;} = null;

    public DateTime CreatedAt {get;set;} = DateTime.UtcNow;

    public ICollection<WatchlistItem> Items {get;set;} = new List<WatchlistItem>();
    
}