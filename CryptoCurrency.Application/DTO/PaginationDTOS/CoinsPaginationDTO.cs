using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.DTO.PaginationDTOS
{
    public class CoinsPaginationDTO
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
