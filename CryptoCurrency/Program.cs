using CryptoCurrency.Application.Interface;
using CryptoCurrency.Application.Mapping;
using CryptoCurrency.Infrastructure.Data;
using CryptoCurrency.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddScoped<CoinSyncService>();
builder.Services.AddScoped<CoinGekoService>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
