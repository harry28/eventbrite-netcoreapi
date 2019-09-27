using eventbrite.Helpers;
using MediatR;

namespace eventbrite.Commands.CreateEvent
{
    public class CreateEventCommand:IRequest<CreateEventCommandViewModel>
    {
        //public string name { get; set; }
        //public DateTime start { get; set; }
        //public DateTime end { get; set; }
        //public string currency { get; set; }

        public NewEvent @event { get; set; }
    }
}
