using Microsoft.AspNetCore.Mvc;
using StockWishlist.Application.Users.DTOs;
using StockWishlist.Application.Users.Interfaces;

namespace StockWishlist.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var user = await _userService.CreateUserAsync(request);
        return Ok(user);
    }
}