using Todo.Core.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Todo.Infrastructure.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public RequestLoggingMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
            var method = context.Request.Method;
            var path = context.Request.Path;
            using (var scope = _scopeFactory.CreateScope())
            {
                var requestLogRepository = scope.ServiceProvider.GetRequiredService<IRequestLogRepository>();
                await requestLogRepository.CreateRequestLog(ipAddress, method, path);
            }
            await _next(context);
        }
    }
}