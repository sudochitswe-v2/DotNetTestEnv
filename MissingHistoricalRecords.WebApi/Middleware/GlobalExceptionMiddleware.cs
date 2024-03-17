using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace MissingHistoricalRecords.WebApi.Middleware
{
    public class GlobalExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                var problemDetails = new ProblemDetails()
                {
                    Detail = ex.Message,
                    Type = "Server Error",
                    Title = "Request got error",
                    Status = (int)HttpStatusCode.InternalServerError,
                };
                var json = JsonSerializer.Serialize(problemDetails);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
        }
    }
}
