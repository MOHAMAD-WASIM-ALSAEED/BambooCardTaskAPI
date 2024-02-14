using AutoMapper;
using BambooCardAPI.Responses;
using BambooCardAPI.Services;
using BambooCardAPI.Swagger.Examples;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;

namespace BambooCardAPI.Endpoint.V1
{
    [ApiController]
    [Route("[controller]")]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsService _hackerNewsService;
        public readonly IMapper _mapper;
        public readonly ICacheService _cacheService;
        public HackerNewsController(IHackerNewsService hackerNewsService, IMapper mapper, ICacheService cacheService)
        {
            _hackerNewsService = hackerNewsService;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        [EnableRateLimiting("HackerNewsLimitterPolicy")]
        [HttpGet("BestStories")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get the top N best stories from Hacker News")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the top N best stories", typeof(IEnumerable<BestStoriesResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status429TooManyRequests)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(HackerNewsStoryDetailsResponseModelExample))]
        public async Task<IActionResult> HackerNewsBestStories([FromQuery][Range(1, int.MaxValue, ErrorMessage = "Count must be greater than 0.")] int? count)
        {
            string cacheKey = "BestStories_" + (count ?? int.MaxValue);

            List<BestStoriesResponse> cachedBestStories = _cacheService.Get<List<BestStoriesResponse>>(cacheKey);
          
            if (cachedBestStories is not null)
            {
                return Ok(cachedBestStories); 
            }

            // if count not specified get all stories 
            var bestStories = await _hackerNewsService.GetTopNBestStoriesAsync(count ?? int.MaxValue);
            var bestStoriesResponse = _mapper.Map<List<BestStoriesResponse>>(bestStories);

            _cacheService.Set(cacheKey, bestStoriesResponse, TimeSpan.FromMinutes(1));

            return Ok(bestStoriesResponse);
        }
    }
}
