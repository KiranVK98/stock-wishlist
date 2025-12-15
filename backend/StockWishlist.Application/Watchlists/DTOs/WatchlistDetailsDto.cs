namespace StockWishlist.Application.Watchlists.DTOs;


public class WatchlistDetailsDto
{
    public Guid WatchlistId {get;set;}
    public string Name {get;set;}
    public List<WatchlistStockDto> Stocks {get;set;}

}


public class WatchlistStockDto
{
    public Guid StockId {get;set;}
    public string Symbol {get;set;}
    public string Name {get;set;}
    public string Sector {get;set;}

    public decimal? TargetPrice {get;set;}
    public decimal? BuyPrice {get;set;}

    public decimal? LivePrice {get;set;}

    public decimal? ProfitLoss => 
        LivePrice.HasValue && BuyPrice.HasValue
            ? LivePrice - BuyPrice
            : null;


    public decimal? DifferenceFromTarget => 
        TargetPrice.HasValue && LivePrice.HasValue
            ? TargetPrice - LivePrice
            : null;


    public bool? TargetReached =>
        TargetPrice.HasValue && LivePrice.HasValue
            ? LivePrice >= TargetPrice
            : null;
}


