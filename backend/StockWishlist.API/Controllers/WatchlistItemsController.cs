using Microsoft.AspNetCore.Mvc;
using StockWishlist.Application.WatchlistItems.DTOs;
using StockWishlist.Application.WatchlistItems.Interfaces;

namespace StockWishlist.API.Controllers;

[ApiController]
[Route("api/watchlist-items")]
public class WatchlistItemsController : ControllerBase
{
    private readonly IWatchListItemService _service;

    public WatchlistItemsController(IWatchListItemService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddStockToWatchListRequest request)
    {
        var item = await _service.AddStockToWatchListAsync(request);
        return Ok(item);
    }
}