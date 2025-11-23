using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using Contracts;

namespace ExceptionHandler
{
    /// <summary>
    /// Class <c>CustomExceptionMiddleware</c>.
    /// </summary>
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILoggerManager _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public CustomExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (BaseCustomException ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                HttpResponse response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await response.WriteAsync(JsonConvert.SerializeObject(new Entities.Dtos.ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "An error has occured",
                    Description = ex.Message
                }));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private async Task HandleExceptionAsync(HttpContext context, BaseCustomException exception)
        {
            HttpResponse response = context.Response;
            BaseCustomException customException = exception;
            int statusCode = customException.Code;
            string message = customException.Message;
            string description = customException.Description;
            response.ContentType = "application/json";
            response.StatusCode = statusCode;
            await response.WriteAsync(JsonConvert.SerializeObject(new Entities.Dtos.ErrorResponse
            {
                StatusCode = statusCode,
                Message = message,
                Description = description
            }));
        }
    }
}