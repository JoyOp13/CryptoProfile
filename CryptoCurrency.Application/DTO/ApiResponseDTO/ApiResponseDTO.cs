using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.DTO.ApiResponseDTO
{
    public class ApiResponseDTO<T>
    {
        public bool Success { get; set; }
        public string Messages { get; set; }
        public T Data { get; set; }
        public string Error { get; set; }
    }
}
