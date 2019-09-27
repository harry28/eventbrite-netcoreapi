using MediatR;

namespace eventbrite.Queries.GetEventById
{
    public class GetEventByIdQuery: IRequest<GetEventByIdQueryViewModel>
    {
        public string EventId { get; set; }
    }
}
