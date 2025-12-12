using Microsoft.AspNetCore.Mvc;
using StockWishlist.Application.Stocks;
using StockWishlist.Application.Stocks.DTOs;
using StockWishlist.Application.Stocks.Interfaces;

namespace StockWishlist.API.Controllers;

[ApiController]
[Route("api/stocks")]
public class StocksController : ControllerBase
{
    private readonly IStockService _service;

    private readonly IStockPriceService _stockPriceService;

    public StocksController(IStockService service, IStockPriceService stockPriceService)
    {
        _service = service;
        _stockPriceService = stockPriceService;
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateStockRequest request)
    {
        var stock = await _service.CreateOrGetStockAsync(request);
        return Ok(stock);
    }

    [HttpGet("price/{symbol}")]
    public async Task<IActionResult> GetPrice(string symbol)
    {
        var price = await _stockPriceService.GetLatestPriceAsync(symbol);

        if(price is null)
        {
            return NotFound(new {message = $"Price not found for symbol - {symbol}"});
        }

        return Ok(new
        {
            symbol = symbol.ToUpper(),
            price
        });
    }


}