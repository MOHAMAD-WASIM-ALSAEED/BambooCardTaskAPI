using AutoMapper;
using BambooCardAPI.Models;
using BambooCardAPI.Responses;

namespace BambooCardAPI.Mapping
{
    public class HackerNewsStoryProfile : Profile
    {
        public HackerNewsStoryProfile()
        {
            CreateMap<HackerNewsStoryDetailsResponseModel, BestStoriesResponse>()
                .ForMember(
                    dest => dest.PostedBy,
                    opt => opt.MapFrom(src => src.By)
                )
                .ForMember(
                    dest => dest.PostedBy,
                    opt => opt.MapFrom(src => src.By)
                )
                .ForMember(
                    dest => dest.Time,
                    opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeSeconds(src.UnixTimestamp))
                )
                 .ForMember(
                    dest => dest.CommentCount,
                    opt => opt.MapFrom(src => src.Descendants)
                )
            .ForMember(
                    dest => dest.Uri,
                    opt => opt.MapFrom(src => src.Url)
                );
        }
    }
}
