using CryptoCurrency.Application.Interface;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Infrastructure.Services
{
    public class LoggerService : ILoggerInterface
    {
        public void LogError(Exception ex, string message)
        {
            Console.WriteLine($"ERROR: {message} - {ex.Message}");
        }

        public void LogInfo(string message)
        {
            Log.Warning(message);
        }

        public void LogWarning(string message)
        {
            Log.Information(message);
        }
    }
}
