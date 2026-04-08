using CryptoCurrency.Application.DTO.ApiResponseDTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.ApiResoponseHelper
{
    public class ApiResponse
    {
        public static IActionResult Success<T>(T data, string message = "Success")
        {
            return new OkObjectResult(new ApiResponseDTO<T>
            {
                Success = true,
                Messages = message,
                Data = data,
                Error = null
            });

        }

        public static IActionResult Failure(string message, string Error = "null")
        {
            return new BadRequestObjectResult(new ApiResponseDTO<string>
            {
                Success = false,
                Messages = message,
                Data = null,
                Error = Error
            });

        }
    }
}
