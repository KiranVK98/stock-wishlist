using StockWishlist.Application.Users.DTOs;
using StockWishlist.Domain.Entities;

namespace StockWishlist.Application.Users.Interfaces;


public interface IUserService
{
    Task<User> CreateUserAsync(CreateUserRequest request);
}