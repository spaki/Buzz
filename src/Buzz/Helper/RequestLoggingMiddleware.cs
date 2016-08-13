using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Buzz.Helper
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RequestLoggingMiddleware> logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            await next.Invoke(context);

            var logTemplate = @"
Client IP: {clientIP}
Request path: {requestPath}
User-Agent: {userAgent}
Start time: {startTime}";

            logger.LogInformation(logTemplate,
            context.Connection.RemoteIpAddress,
            UriHelper.GetDisplayUrl(context.Request),
            context.Request.Headers["User-Agent"],
            DateTime.Now);
        }
    }
}
