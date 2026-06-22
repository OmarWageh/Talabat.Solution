using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Text.Json;
using Talabat.Api.Errors;

namespace Talabat.Api.CustomMiddleware
{
    //i do this Middleware to handle the throw exception when the endpoint output servererror
    public class HandleTheServerErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HandleTheServerErrorMiddleware> _loger;
        private readonly IHostEnvironment _environment;

        public HandleTheServerErrorMiddleware(RequestDelegate next,ILogger<HandleTheServerErrorMiddleware>loger,IHostEnvironment environment) 
        {
            _next = next;
           _loger = loger;
           _environment = environment;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);

            }
            catch (Exception ex) 
            {
                _loger.LogError(ex,ex.Message); //logger in development

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = 500;

                var response = _environment.IsDevelopment()
                  ? new ApiResponseToServerError(500,ex.Message,ex.StackTrace?.ToString())
                 : new ApiResponseToServerError(500);
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);

                await httpContext.Response.WriteAsync(json);
               
            }
        }
    }
}
