using StockWishlist.Application.Stocks.DTOs;
using StockWishlist.Domain.Entities;

namespace StockWishlist.Application.Stocks.Interfaces;

public interface IStockService
{
    Task<Stock> CreateOrGetStockAsync(CreateStockRequest request);

}