using System.Collections.Generic;
using MediatR;

namespace eventbrite.Queries.GetEvents
{
    public class GetEventsQuery: IRequest<List<GetEventsQueryViewModel>>
    {
    }
}
