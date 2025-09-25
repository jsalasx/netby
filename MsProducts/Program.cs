using Microsoft.EntityFrameworkCore;
using MsProducts.Application.UseCase;
using MsProducts.Domain.Ports;
using MsProducts.Infrastructure.Persistence;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string? connString = Environment.GetEnvironmentVariable("DB_CONNECTION");
// Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connString ?? builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Use Cases
builder.Services.AddScoped<AddProductUseCase>();
builder.Services.AddScoped<GetProductByIdUseCase>();
builder.Services.AddScoped<GetProductsByFilterUseCase>();
builder.Services.AddScoped<UpdateProductUseCase>();
builder.Services.AddScoped<DeleteProductUseCase>();
builder.Services.AddScoped<UpdateStockUseCase>();

string? redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION");

var multiplexers = new List<RedLockMultiplexer>
{
    ConnectionMultiplexer.Connect(redisConnectionString ?? "localhost:6379")
};


builder.Services.AddSingleton<RedLockFactory>(_ =>
{
    return RedLockFactory.Create(multiplexers);
});



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map controllers
app.MapControllers();

app.Run();
