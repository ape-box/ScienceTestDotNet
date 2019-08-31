using System;
using System.Net.Http;
using Elastic.Apm.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScienceTest.Api.Clients;
using ScienceTest.Api.Logging;
using App.Metrics;
using Microsoft.Extensions.Logging;
using ScienceTest.Api.Clients.Experiments;

namespace ScienceTest.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var apiHostSettings = Configuration.GetSection(nameof(ScienceApiSettings)).Get<ScienceApiSettings>();
            
            services.AddHttpClient();
            services.AddHttpClient(ApiToUseClient.ClientName, config =>
            {
                config.BaseAddress = apiHostSettings.UpstreamApiUrl;
            });
            services.AddHttpClient(ApiToTestClient.ClientName, config =>
            {
                config.BaseAddress = apiHostSettings.UpstreamApiUrlAlt;
            });

//            services.AddScoped<IUpstreamApiClient, ApiToUseClient>();
            services.AddScoped<IUpstreamApiClient>(serviceProvider => new UpstreamApiClientExperiment(
                new ApiToUseClient(serviceProvider.GetRequiredService<IHttpClientFactory>(), serviceProvider.GetRequiredService<ILogger<ApiToUseClient>>()),
                new ApiToTestClient(serviceProvider.GetRequiredService<IHttpClientFactory>(), serviceProvider.GetRequiredService<ILogger<ApiToTestClient>>()),
                serviceProvider.GetRequiredService<IMetrics>(),
                serviceProvider.GetRequiredService<ILogger<UpstreamApiClientExperiment>>()));

            var metrics = new MetricsBuilder()
                .Configuration.Configure(options =>
                {
                    options.Enabled = true;
                    options.ReportingEnabled = true;
                })
                .Report.ToInfluxDb(options => {
                    options.InfluxDb.BaseUri = new Uri("http://127.0.0.1:8086");
                    options.InfluxDb.Database = "logDb";
                    options.InfluxDb.CreateDataBaseIfNotExists = true;
                })
                .Build();
            services.AddMetrics(metrics);
            services.AddMetricsTrackingMiddleware();
            services.AddMetricsReportingHostedService();

            services
                .AddMvc()
                .AddMetrics()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseMiddleware<LoggingMiddleware>()
                .UseElasticApm(Configuration)
                .UseMetricsAllMiddleware()
                .UseMvc();
        }
    }
}
