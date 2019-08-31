using System;
using App.Metrics.AspNetCore;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace ScienceTest.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("CustomPropertyEnrichment", "veryenriched")
//                    .WriteTo.Console(new ElasticsearchJsonFormatter())
                    .WriteTo.Console()
//                    .WriteTo.InfluxDB("ApiScientist2", "http://localhost", "logDb")
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200")) {
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6}))
//                .UseMetrics()
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}
