using StockWishlist.Application.Stocks;
using System.Net.Http;
using Microsoft.Extensions.Options;

using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;


namespace StockWishlist.Infrastructure.Stocks;

public class StockPriceService : IStockPriceService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    public StockPriceService(IHttpClientFactory httpClientFactory, IOptions<AlphaVantageSettings> settings)
    {
        _httpClient = httpClientFactory.CreateClient("AlphaVantage");
        _apiKey = settings.Value.ApiKey;
    }


    public async Task<decimal?> GetLatestPriceAsync(string symbol)
    {
        var url = $"query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={_apiKey}";

        var response = await _httpClient.GetAsync(url);

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
                    return price;
                }
            }
        }

        return null;
    } 
}