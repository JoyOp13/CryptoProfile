using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Infrastructure.Services
{
    public class CoinSyncService : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;

        public CoinSyncService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
               
                using var scope = scopeFactory.CreateScope();

                var coinService = scope.ServiceProvider.GetRequiredService<CoinGekoService>();

                Console.WriteLine("Syncing coins");

                await coinService.SyncCoins();

                Console.WriteLine("Sync Done");

                // 2 minutes delay before the next sync
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }
    }
}
