using System;

namespace eventbrite.Commands.CreateEvent
{
    public class CreateEventCommandViewModel
    {
        public string name { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string id { get; set; }
    }
}
