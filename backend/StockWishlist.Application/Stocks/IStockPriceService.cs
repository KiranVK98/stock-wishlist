namespace StockWishlist.Application.Stocks;

public interface IStockPriceService
{
    Task<decimal?> GetLatestPriceAsync(string symbol);
}