using System.Threading.Tasks;
using eventbrite.Commands.CreateEvent;
using eventbrite.Helpers;

namespace eventbrite.Services.Interfaces
{
    public interface IEventbriteService
    {
        Task<EventbriteEvents> GetEvents();

        Task<Event> GetEventById(string eventId);

        Task<Event> CreateEvent(CreateEventCommand newEvent);
    }
}
