using BambooCardAPI.HttpClients;
using BambooCardAPI.Models;

namespace BambooCardAPI.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HackerAPIHttpClient _hackerAPIHttpClient;
        public HackerNewsService(HackerAPIHttpClient hackerAPIHttpClient)
        {
            _hackerAPIHttpClient = hackerAPIHttpClient;
        }

        public async Task<List<HackerNewsStoryDetailsResponseModel>> GetTopNBestStoriesAsync(int storiesCount)
        {
            var bestStoriesIds = await _hackerAPIHttpClient.GetBestStoriesIdsAsync();
            var storiesDetails = await GetTopScoredStoriesDetailsAsync(bestStoriesIds, storiesCount);
            return storiesDetails;
        }

        public async Task<List<HackerNewsStoryDetailsResponseModel>> GetTopScoredStoriesDetailsAsync(IEnumerable<int> storiesIds, int count)
        {
            var tasks = storiesIds.Select(storyId => _hackerAPIHttpClient.GetStoryDetailsAsync(storyId));
            var allStoriesDetails = await Task.WhenAll(tasks);
            var mostCommentedStories = allStoriesDetails.Where(details => details != null)
                                       .OrderByDescending(story => story.Score)
                                       .Take(count)
                                       .ToList();

            return mostCommentedStories
                ?? Enumerable.Empty<HackerNewsStoryDetailsResponseModel>().ToList();

        }

    }
}
