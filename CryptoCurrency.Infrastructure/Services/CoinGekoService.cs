using CryptoCurrency.Application.DTO.CoinGeckoDTO;
using CryptoCurrency.Application.DTO.PaginationDTOS;
using CryptoCurrency.Domain.Models;
using CryptoCurrency.Infrastructure.Data;
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

        public CoinGekoService(HttpClient http, ApplicationDbContext db)
        {
            this.http = http;
            this.db = db;
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
                var exists = db.CryptoCoin
                    .FirstOrDefault(x => x.CoinGeckoId == coin.id);

                if (exists == null)
                {
                    db.CryptoCoin.Add(new CryptoCoin
                    {
                        CryptoCoinName = coin.name,
                        CryptoSymbol = coin.symbol,
                        CryptoIcon = coin.image,
                        CoinGeckoId = coin.id
                    });
                }
            }

            await db.SaveChangesAsync();
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
              $"?vs_currency=usd&per_page={dto.PageSize}&page={dto.Page}";

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
    }
}
