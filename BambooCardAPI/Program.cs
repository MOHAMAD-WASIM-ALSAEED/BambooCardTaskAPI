using BambooCardAPI.ExceptionHandlers;
using BambooCardAPI.HealthChecks;
using BambooCardAPI.HttpClients;
using BambooCardAPI.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Threading.RateLimiting;
namespace BambooCardAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hacker News API", Version = "v1" });

                // Add examples to Swagger
                c.ExampleFilters();
            });
            builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

            builder.Services.AddHealthChecks().AddCheck<BestStoriesHealthCheck>("Hacker News API");

            builder.Services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = 429;
                options.AddPolicy("HackerNewsLimitterPolicy", httpContext =>
                {
                    var IpAddress = httpContext.Connection.RemoteIpAddress?.ToString();

                    if (IpAddress != null)
                    {
                        // allows each IP address for 5 requests max in 1 minute 
                        return RateLimitPartition.GetFixedWindowLimiter(IpAddress,
                        partition => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1)
                        });
                    }
                    else
                    {
                        return RateLimitPartition.GetNoLimiter("");
                    }
                });

            });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
             .WaitAndRetryAsync(4, retryAttempts => TimeSpan.FromSeconds(2));

            var basicCircuitBreakerPolicy = Policy
                   .Handle<HttpRequestException>()
                   .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
               .CircuitBreakerAsync(12, TimeSpan.FromSeconds(60));

            var HackerAPIUrl = builder.Configuration.GetValue<string>("ExternalAPIURL:HackerNews") ?? throw new InvalidOperationException("HackerAPIUrl is null");

            builder.Services.AddHttpClient<HackerAPIHttpClient>(client =>
            {
                client.BaseAddress = new Uri(HackerAPIUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(basicCircuitBreakerPolicy);

            builder.Services.AddScoped<IHackerNewsService, HackerNewsService>();
            builder.Services.AddScoped<ICacheService, InMemoryCacheService>();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();
            builder.Services.AddMemoryCache();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseExceptionHandler();
            app.UseHttpsRedirection();

            app.UseHealthChecks("/healthy", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseAuthorization();

            app.UseRateLimiter();
            app.MapControllers();

            app.Run();
        }
    }
}
