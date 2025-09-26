using MsTransactions.Infrastructure.Adapters;
using Microsoft.EntityFrameworkCore;
using MsTransactions.Infrastructure.Persistence;
using MsTransactions.Infrastructure.Persistence.Repositories;
using MsTransactions.Domain.Port;
using MsTransactions.Application.UseCase;

var builder = WebApplication.CreateBuilder(args);
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy  =>
        {
            policy.WithOrigins("*") // URL de tu frontend Angular
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

string? connString = Environment.GetEnvironmentVariable("DB_CONNECTION");
// Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connString ?? builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
// Use Cases
builder.Services.AddScoped<AddTransactionUseCase>();
builder.Services.AddScoped<GetTransactionByIdUseCase>();
builder.Services.AddScoped<GetTransactionByFilterUseCase>();
builder.Services.AddScoped<DeleteTransactionUseCase>();

string? productsApiUrl = Environment.GetEnvironmentVariable("URL_PRODUCTS_API");
builder.Services.AddHttpClient<ProductApiClient>(client =>
{
    client.BaseAddress = new Uri(productsApiUrl ?? "http://msproducts"); // URL base del micro de productos
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(myAllowSpecificOrigins);
// Map controllers
app.MapControllers();

app.Run();