using AutoMapper;
using eventbrite.Helpers;
using eventbrite.Queries.GetEventById;

namespace eventbrite.Profiles
{
    public class EventMappingProfile:Profile
    {
        public EventMappingProfile()
        {
            //CreateMap<Source,Destination>
            CreateMap<Event, GetEventByIdQueryViewModel>()
                .ForMember(dest => dest.name, source => source.MapFrom(src => src.name.html.ToString()))
                .ForMember(dest => dest.description, source => source.MapFrom(src => src.description.html))
                .ForMember(dest => dest.start, source => source.MapFrom(src => src.start.local))
                .ForMember(dest => dest.end, source => source.MapFrom(src => src.end.local));

        }
    }
}
