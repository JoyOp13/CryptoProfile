using CryptoCurrency.Application.Interface;
using CryptoCurrency.Application.Mapping;
using CryptoCurrency.Infrastructure.Data;
using CryptoCurrency.Infrastructure.Services;
using CryptoCurrencyAPI.ExceptionHelper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("dbconn")));

// For Avoiding 403 Error 
builder.Services.AddHttpClient<CoinGekoService>(client =>
{
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
});
builder.Services.AddAutoMapper(typeof(DTOMapping));
builder.Services.AddScoped<IWalletInterface, WalletService>();
builder.Services.AddScoped<ICryptoTransactionInterface, CryptoTransactionService>();
builder.Services.AddScoped<IPortfolioInterface, PortfolioResService>(); 
builder.Services.AddScoped<ILoggerInterface,LoggerService>();
builder.Services.AddScoped<CoinSyncService>();
builder.Services.AddScoped<CoinGekoService>();

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>
    (context => RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString(),
        factory: _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 5,
            Window = TimeSpan.FromSeconds(10),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            //QueueLimit=2
        }));

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsJsonAsync(
            new
            {
                success = false,
                message = "Bas kar Lala Server Break Karega kya",
                data = (object)null,
                error = "Rate limit exceeded"
            });
    };
});


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Warning()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// For Accessing CoinGeko Api Data
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHelper>();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

app.Run();
