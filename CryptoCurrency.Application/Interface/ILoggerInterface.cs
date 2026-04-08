using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.Interface
{
    public interface ILoggerInterface
    {
        void LogError(Exception ex, string message);
        void LogWarning(string message);
        void LogInfo(string message);
    }
}
