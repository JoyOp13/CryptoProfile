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
            throw new NotImplementedException();
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
