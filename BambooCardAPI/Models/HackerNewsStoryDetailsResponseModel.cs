using System.Text.Json.Serialization;

namespace BambooCardAPI.Models
{
    public record HackerNewsStoryDetailsResponseModel
    {
        [JsonPropertyName("by")]
        public string By { get; init; }

        [JsonPropertyName("descendants")]
        public int Descendants { get; init; }

        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("kids")]
        public List<int> Kids { get; init; }

        [JsonPropertyName("score")]
        public int Score { get; init; }


        [JsonPropertyName("time")]
        public int UnixTimestamp { get; init; }

        [JsonPropertyName("title")]
        public string Title { get; init; }

        [JsonPropertyName("type")]
        public string Type { get; init; }

        [JsonPropertyName("url")]
        public string Url { get; init; }
    }
}
