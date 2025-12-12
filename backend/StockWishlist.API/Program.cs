using Microsoft.EntityFrameworkCore;
using StockWishlist.Application.Stocks;
using StockWishlist.Application.Stocks.Interfaces;
using StockWishlist.Application.Users.Interfaces;
using StockWishlist.Application.WatchlistItems.Interfaces;
using StockWishlist.Application.Watchlists.Interfaces;
using StockWishlist.Infrastructure;
using StockWishlist.Infrastructure.Data;
using StockWishlist.Infrastructure.Stocks;
using StockWishlist.Infrastructure.Users;
using StockWishlist.Infrastructure.WatchlistItems;
using StockWishlist.Infrastructure.Watchlists;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options=> 
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))); 

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWatchlistService, WatchlistService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IWatchListItemService, WatchListItemService>();
builder.Services.AddControllers();

builder.Services.AddHttpClient("AlphaVantage", client =>
{
    client.BaseAddress = new Uri("https://www.alphavantage.co/");
});

builder.Services.Configure<AlphaVantageSettings>(
    builder.Configuration.GetSection("AlphaVantage")
);

builder.Services.AddInfrastructure();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
