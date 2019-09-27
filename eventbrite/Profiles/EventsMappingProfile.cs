using AutoMapper;
using eventbrite.Helpers;
using eventbrite.Queries.GetEvents;

namespace eventbrite.Profiles
{
    public class EventsMappingProfile : Profile
    {
        public EventsMappingProfile()
        {
            //CreateMap<Source,Destination>
            CreateMap<Event, GetEventsQueryViewModel>();
        }
    }
}
