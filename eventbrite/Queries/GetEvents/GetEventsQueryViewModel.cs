using eventbrite.Helpers;

namespace eventbrite.Queries.GetEvents
{
    public class GetEventsQueryViewModel
    {
        public Name name { get; set; }
        public Description description { get; set; }
        public Start start { get; set; }
        public End end { get; set; }
    }
}
