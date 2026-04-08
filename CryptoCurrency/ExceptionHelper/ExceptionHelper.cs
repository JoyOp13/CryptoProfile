using CryptoCurrency.Application.DTO.ApiResponseDTO;
using CryptoCurrency.Application.Interface;

namespace CryptoCurrencyAPI.ExceptionHelper
{
    public class ExceptionHelper
    {
        private readonly RequestDelegate rd;
        public ExceptionHelper(RequestDelegate rd)
        {
            this.rd = rd;
           
        }
        public async Task Invoke(HttpContext context, ILoggerInterface logger)
        {
            try
            {
                await rd(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled Exception");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = new ApiResponseDTO<string>
                {
                    Success = false,
                    Messages = "Something went wrong",
                    Data = null,
                    Error = ex.InnerException?.Message ?? ex.Message
                };

                var json = System.Text.Json.JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);
            }

        }
    }
}
