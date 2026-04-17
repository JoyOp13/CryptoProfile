using AutoMapper;
using CryptoCurrency.Application.ApiResoponseHelper;
using CryptoCurrency.Application.DTO.CoinGeckoDTO;
using CryptoCurrency.Application.DTO.FavoriteDTO;
using CryptoCurrency.Application.DTO.PaginationDTOS;
using CryptoCurrency.Domain.Models;
using CryptoCurrency.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CryptoCurrency.Infrastructure.Services
{
    public class CoinGekoService
    {
        private readonly HttpClient http;
        private readonly ApplicationDbContext db;
        private IMapper mapper;

        public CoinGekoService(HttpClient http, ApplicationDbContext db, IMapper mapper)
        {
            this.http = http;
            this.db = db;
            this.mapper= mapper;
        }

        public async Task SyncCoins()
        {
            var coins = await GetCoins(new CoinsPaginationDTO
            {
                Page = 1,
                PageSize = 30
            });

            foreach (var coin in coins)
            {
                if (coin == null || string.IsNullOrEmpty(coin.id))
                    continue;
                Console.WriteLine($"Processing: {coin.id}");
                var exists = await db.CryptoCoin.FirstOrDefaultAsync(x => x.CoinGeckoId == coin.id);

                Console.WriteLine($"Coin: {coin.id}, Price: {coin.current_price}");
                var name = coin.name ?? "Unknown";
                var symbol = coin.symbol ?? "NA";
                var image = coin.image ?? "";
                var price = coin.current_price != null ? (decimal)coin.current_price : 0;
                if (exists == null)
                {
                    db.CryptoCoin.Add(new CryptoCoin
                    {

                        CryptoCoinName = name,
                        CryptoSymbol = symbol,
                        CryptoIcon = image,
                        CoinGeckoId = coin.id,
                        CurrentPrice = price,
                        Quantity = 0,
                        LastUpdated = DateTime.Now,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "System"
                    });
                    Console.WriteLine("Adding...");
                    Console.WriteLine("Adding new coin: " + coin.id);
                }
                else
                {
                    Console.WriteLine("Already exists...");
                    exists.CryptoCoinName = name;
                    exists.CryptoSymbol = symbol;
                    exists.CryptoIcon = image;
                    exists.CurrentPrice = price;
                    exists.LastUpdated = DateTime.Now;
                    exists.ModifiedAt = DateTime.Now;
                    exists.ModifiedBy = "System";
                }
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }
        public async Task<decimal> GetCoinPrice(string coinId)
        {
            var url = $"https://api.coingecko.com/api/v3/simple/price?ids={coinId}&vs_currencies=usd";

            http.DefaultRequestHeaders.Clear();
            //http.DefaultRequestHeaders.Add("Accept", "application/json");
            http.DefaultRequestHeaders.Add("User-Agent", "CryptoApp/1.0 (Jay)");

            var response = await http.GetAsync(url);

            if (!response.IsSuccessStatusCode)

                throw new Exception("Price Not Featched");

            var json = await response.Content.ReadAsStringAsync();

            var data = JsonDocument.Parse(json);

            var price = data.RootElement
                .GetProperty(coinId)
                .GetProperty("usd")
                .GetDecimal();

            return price;
        }
        public async Task<List<CoinGeckoDTO>> GetCoins(CoinsPaginationDTO dto)
        {
            var url = $"https://api.coingecko.com/api/v3/coins/markets" +
          $"?vs_currency=usd&order=market_cap_desc&per_page={dto.PageSize}&page={dto.Page}&sparkline=false";

            http.DefaultRequestHeaders.Clear();
            //http.DefaultRequestHeaders.Add("Accept", "application/json");
            http.DefaultRequestHeaders.Add("User-Agent", "CryptoApp/1.0 (Jay)");

            var response = await http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }

            var json = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<List<CoinGeckoDTO>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return data;
        }

        //public async Task<List<CoinFeatchDTO>> GetCoinsData(int pageSize) {

        //    var data = db.CryptoCoin.Take(pageSize).ToList();
        //    var coinData = mapper.Map<List<CoinFeatchDTO>>(data);
                       
        //    return coinData;
        //}

        public async Task<List<CoinFeatchDTO>> GetCoinsData()
        {

            var data = db.CryptoCoin.ToList();
            var coinData = mapper.Map<List<CoinFeatchDTO>>(data);

            return coinData;
        }
    }
}
