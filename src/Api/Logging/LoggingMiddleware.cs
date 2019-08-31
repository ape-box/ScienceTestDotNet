using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ScienceTest.Api.Logging
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var CorrelationId = context.AddCorrelationId();
            var method = context.Request.Method;
            var requestPath = context.Request.Path.ToString();

            try
            {
                await _next(context);

                var elapsedTime = stopwatch.ElapsedMilliseconds;
                _logger.LogInformation("Success for {@method} {@requestPath} with {@CorrelationId} in {@elapsedTime} millis", method, requestPath, CorrelationId, elapsedTime);
            }
            catch (Exception exception)
            {
                var elapsedTime = stopwatch.ElapsedMilliseconds;
                _logger.LogInformation("Exception {@exception.Message} for {@method} {@requestPath} with {@CorrelationId} in {@elapsedTime} millis", exception.Message, method, requestPath, CorrelationId, elapsedTime);
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}
