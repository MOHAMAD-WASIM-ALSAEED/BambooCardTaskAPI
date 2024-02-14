using BambooCardAPI.Models;

namespace BambooCardAPI.Services
{
    public interface IHackerNewsService
    {
        Task<List<HackerNewsStoryDetailsResponseModel>> GetTopScoredStoriesDetailsAsync(IEnumerable<int> storiesIds, int count = int.MaxValue);
        Task<List<HackerNewsStoryDetailsResponseModel>> GetTopNBestStoriesAsync(int storiesCount);
    }
}