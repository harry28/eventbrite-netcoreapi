using System;

namespace eventbrite.Queries.GetEventById
{
    public class GetEventByIdQueryViewModel
    {
        public string name { get; set; }
        public string description { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string currency { get; set; }
    }
}
