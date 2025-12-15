using StockWishlist.Application.Stocks;
using System.Net.Http;
using Microsoft.Extensions.Options;

using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;


namespace StockWishlist.Infrastructure.Stocks;

public class StockPriceService : IStockPriceService
{
    private readonly string _apiKey;

    private readonly IHttpClientFactory _httpClientFactory;

    private readonly IMemoryCache _cache;
    public StockPriceService(IHttpClientFactory httpClientFactory, IOptions<AlphaVantageSettings> settings, IMemoryCache cache)
    {
        _httpClientFactory = httpClientFactory;
        _apiKey = settings.Value.ApiKey;
        _cache = cache;
    }


    public async Task<decimal?> GetLatestPriceAsync(string symbol)
    {
        symbol = symbol.ToUpper();

        string cacheKey = $"stock_price_{symbol}";

        if(_cache.TryGetValue(cacheKey, out decimal cachedPrice))
        {
            return cachedPrice;
        }
        var client = _httpClientFactory.CreateClient("AlphaVantage");
        var url = $"query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={_apiKey}";

        var response = await client.GetAsync(url);

        if(!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(json);

        var root = document.RootElement;

        if(root.TryGetProperty("Global Quote", out var quote))
        {
            if(quote.TryGetProperty("05. price", out var priceElement))
            {
                var priceString = priceElement.GetString();
                if (decimal.TryParse(priceString, out var price))
                {
                    _cache.Set(cacheKey, price, TimeSpan.FromSeconds(30));
                    return price;
                }
            }
        }

        return null;
    } 
}