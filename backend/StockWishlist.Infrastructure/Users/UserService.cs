using StockWishlist.Application.Users.DTOs;
using StockWishlist.Application.Users.Interfaces;
using StockWishlist.Domain.Entities;
using StockWishlist.Infrastructure.Data;

namespace StockWishlist.Infrastructure.Users;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUserAsync(CreateUserRequest request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FullName = request.FullName,
            PasswordHash = "TEMP-PASSWORD-HASH"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }
}