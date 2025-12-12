namespace StockWishlist.Application.Stocks.DTOs;


public class CreateStockRequest
{
    public string Symbol {get;set;} = string.Empty;
    public string Name {get;set;} = string.Empty;

    public string Sector {get;set;} = string.Empty;
}