using AutoMapper;
using eventbrite.Commands.CreateEvent;
using eventbrite.Helpers;

namespace eventbrite.Profiles
{
    public class CreateEventMappingProfile : Profile
    {
        public CreateEventMappingProfile()
        {
            //CreateMap<Source,Destination>
            CreateMap<Event, CreateEventCommandViewModel>()
                .ForMember(dest => dest.name, source => source.MapFrom(src => src.name.html.ToString()))
                .ForMember(dest => dest.start, source => source.MapFrom(src => src.start.local))
                .ForMember(dest => dest.end, source => source.MapFrom(src => src.end.local))
                .ForMember(dest => dest.id, source => source.MapFrom(src => src.id));
        }
    }
}
