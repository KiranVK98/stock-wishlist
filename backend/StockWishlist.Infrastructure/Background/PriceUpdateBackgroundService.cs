using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StockWishlist.Application.Stocks;
using StockWishlist.Infrastructure.Data;

namespace StockWishlist.Infrastructure.Background;

public class PriceUpdateBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PriceUpdateBackgroundService> _logger;

    private static readonly TimeSpan RefreshInterval = TimeSpan.FromMinutes(1);
    private static readonly TimeSpan PerSymbolDelay = TimeSpan.FromSeconds(15);

    public PriceUpdateBackgroundService(IServiceProvider serviceProvider, ILogger<PriceUpdateBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PriceUpdateBackgroundService started.");

        while(!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RefreshAllPrices(stoppingToken);
            }
            catch(Exception ex) when(!stoppingToken.IsCancellationRequested)
            {
                _logger.LogError(ex, "Error while fetching stock prices");
            }

            try
            {
                await Task.Delay(RefreshInterval, stoppingToken);
            }
            catch(TaskCanceledException)
            {
                
            }

        }
        _logger.LogInformation("Price Update Background service stopping");


    }

    private async Task RefreshAllPrices(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var priceService = scope.ServiceProvider.GetRequiredService<IStockPriceService>();

        var symbols = await context.Stocks
            .Select(s => s.Symbol)
            .Where(s => s != null && s != "")
            .Distinct()
            .ToListAsync(stoppingToken);

        if(symbols.Count == 0)
        {
            _logger.LogInformation("No symbols found to refresh");
            return;
        }

        _logger.LogInformation($"Refreshing symbols for count - {symbols.Count}");

        foreach(var symbol in symbols)
        {
            if(stoppingToken.IsCancellationRequested)
                break;

            try
            {
                var price = await priceService.GetLatestPriceAsync(symbol);

                if(price.HasValue)
                {
                    _logger.LogInformation($"Updated Symbol - {symbol} to price - {price}");
                }
                else
                {
                    _logger.LogWarning($"No price returned for symbol - {symbol}");
                }
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex, $"Error updating price for symbol - {symbol}");
            }

            //Wait between symbol - alphavantage 5 requests per minute
            try
            {
                await Task.Delay(PerSymbolDelay, stoppingToken);
            }
            catch(TaskCanceledException)
            {
                break;
            }
        }
    }

}