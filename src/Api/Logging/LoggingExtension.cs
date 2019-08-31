using System;
using Microsoft.AspNetCore.Http;

namespace ScienceTest.Api.Logging
{
    public static class LoggingExtension
    {
        private const string CorrelationIdName = "CorrelationId";
        
        public static Guid AddCorrelationId(this HttpContext context, Guid? existingId = null)
        {
            var correlationId = existingId ?? Guid.NewGuid();
            
            context.Items.Add(CorrelationIdName, correlationId);

            return correlationId;
        }
        
        public static Guid GetCorrelationId(this HttpContext context) =>
            context.Items.ContainsKey(CorrelationIdName)
                ? (Guid) context.Items[CorrelationIdName]
                : AddCorrelationId(context);
    }
}
