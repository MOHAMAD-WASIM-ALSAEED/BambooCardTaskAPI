using BambooCardAPI.Models;

namespace BambooCardAPI.HttpClients
{
    public class HackerAPIHttpClient
    {
        private readonly HttpClient _client;
        public HackerAPIHttpClient(HttpClient client)
        {
            _client = client;
        }


        public async Task<IEnumerable<int>> GetBestStoriesIdsAsync()
        {
                var bestStories = await _client.GetFromJsonAsync<IEnumerable<int>>("beststories.json");
                return bestStories ?? Enumerable.Empty<int>();
        }

        public async Task<HackerNewsStoryDetailsResponseModel> GetStoryDetailsAsync(int storyId)
        {
            var storyDetail = await _client.GetFromJsonAsync<HackerNewsStoryDetailsResponseModel>($"item/{storyId}.json");
            return storyDetail;
        }
    }
}
