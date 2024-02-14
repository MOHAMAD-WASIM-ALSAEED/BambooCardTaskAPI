using System;
using System.Collections.Generic;
using BambooCardAPI.Models;
using BambooCardAPI.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace BambooCardAPI.Swagger.Examples
{
    public class HackerNewsStoryDetailsResponseModelExample : IExamplesProvider<IEnumerable<BestStoriesResponse>>
    {
        public IEnumerable<BestStoriesResponse> GetExamples()
        {
            return new List<BestStoriesResponse>
            {
                new BestStoriesResponse
                {
                    Title = "Sample Title",
                    Uri = "http://example.com",
                    PostedBy = "Sample User",
                    Time = DateTime.UtcNow, // Current UTC time
                    Score = 100,
                    CommentCount = 50
                }
            };
        }
    }
}
