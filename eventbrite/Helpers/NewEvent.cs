using System;

namespace eventbrite.Helpers
{
    public class CreateEvent
    {
        public NewEvent @event { get; set; }
    }

    public class NewName
    {
        public string html { get; set; }
    }

    public class NewStart
    {
        public string timezone { get; set; }
        public DateTime utc { get; set; }
    }

    public class NewEnd
    {
        public string timezone { get; set; }
        public DateTime utc { get; set; }
    }

    public class NewEvent
    {
        public NewName name { get; set; }
        public NewStart start { get; set; }
        public NewEnd end { get; set; }
        public string currency { get; set; }
    }

    
}
