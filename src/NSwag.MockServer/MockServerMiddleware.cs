using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NSwag.MockServer
{
    public class MockServerMiddleware
    {
        private readonly RequestDelegate _next;

        public MockServerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.WriteAsync("Hello, Mock Server!"); 
        } 
    }
}