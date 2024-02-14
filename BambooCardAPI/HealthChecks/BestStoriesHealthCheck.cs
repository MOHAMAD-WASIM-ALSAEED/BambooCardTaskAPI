using BambooCardAPI.HttpClients;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BambooCardAPI.HealthChecks
{
    public class BestStoriesHealthCheck : IHealthCheck
    {
        private readonly HackerAPIHttpClient _client;

        public BestStoriesHealthCheck(HackerAPIHttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _client.GetBestStoriesIdsAsync();
                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Error: {ex.Message}");
            }
        }
    }
}
