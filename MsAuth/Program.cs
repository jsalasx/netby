

using MsAuth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MsAuth.Domain.Ports;
using MsAuth.Application.UseCase;
using Shared.Infrastructure;

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
        policy =>
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

// Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();

string? jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
string? jwtRefreshSecret = Environment.GetEnvironmentVariable("JWT_REFRESH_SECRET");
int jwtExpiresInMinutes = Environment.GetEnvironmentVariable("JWT_EXPIRES_IN_MINUTES") != null ?
    int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRES_IN_MINUTES")!) : 60;

var jwtSettings = new JwtSettings
{
    Secret = jwtSecret ?? "default_secret_key",
    RefreshSecret = jwtRefreshSecret ?? "default_refresh_secret_key",
    ExpiresInMinutes = jwtExpiresInMinutes

};
builder.Services.AddSingleton(jwtSettings);

// Use Cases
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RegisterUseCase>();


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
