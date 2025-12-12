using Microsoft.EntityFrameworkCore;
using StockWishlist.Application.Stocks.DTOs;
using StockWishlist.Application.Stocks.Interfaces;
using StockWishlist.Domain.Entities;
using StockWishlist.Infrastructure.Data;

namespace StockWishlist.Infrastructure.Stocks;

public class StockService : IStockService
{
    private readonly AppDbContext _context;

    public StockService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Stock> CreateOrGetStockAsync(CreateStockRequest request)
    {
        var existing = await _context.Stocks
            .FirstOrDefaultAsync(s => s.Symbol == request.Symbol);

        if(existing != null)
        {
            return existing;
        }

        var stock = new Stock
        {
            Id = Guid.NewGuid(),
            Symbol = request.Symbol,
            Name = request.Name,
            Sector = request.Sector
        };

        _context.Stocks.Add(stock);

        await _context.SaveChangesAsync();

        return stock;
    }
}