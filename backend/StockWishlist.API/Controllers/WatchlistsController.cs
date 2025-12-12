using Microsoft.AspNetCore.Mvc;
using StockWishlist.Application.Watchlists.DTOs;
using StockWishlist.Application.Watchlists.Interfaces;

namespace StockWishlist.API.Controllers;

[ApiController]
[Route("api/watchlists")]
public class WatchlistsController : ControllerBase
{
    private readonly IWatchlistService _watchlistService;

    public WatchlistsController(IWatchlistService watchlistService)
    {
        _watchlistService = watchlistService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWatchlistRequest request)
    {
        var watchList = await _watchlistService.CreateWatchlistAsync(request);
        return Ok(watchList);
    }

    [HttpGet("{watchlistId:guid}")]
    public async Task<IActionResult> Get(Guid watchlistId)
    {
        var result = await _watchlistService.GetWatchlistWithStocksAsync(watchlistId);

        if(result == null)
        {
            return NotFound("Watchlist not found");
        }

        return Ok(result);
    }
}