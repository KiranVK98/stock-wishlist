using Microsoft.Extensions.DependencyInjection;
using StockWishlist.Application.Stocks;
using StockWishlist.Infrastructure.Stocks;

namespace StockWishlist.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IStockPriceService, StockPriceService>();

        return services;
    }
}