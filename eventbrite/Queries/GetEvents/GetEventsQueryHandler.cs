using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using eventbrite.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace eventbrite.Queries.GetEvents
{
    public class GetEventsQueryHandler: IRequestHandler<GetEventsQuery, List<GetEventsQueryViewModel>>
    {
        private readonly IMediator _mediator;
        private ILogger<GetEventsQueryHandler> _logger;
        private readonly IEventbriteService _eventbriteService;
        private IMapper _mapper;

        public GetEventsQueryHandler(IMediator mediator, ILogger<GetEventsQueryHandler> logger, IEventbriteService eventbriteService,IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _eventbriteService = eventbriteService;
            _mapper = mapper;
        }

        public async Task<List<GetEventsQueryViewModel>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {
            List<GetEventsQueryViewModel> viewModel = new List<GetEventsQueryViewModel>();
            try
            {
                var response = await _eventbriteService.GetEvents();
                if (response.Events.Any())
                {
                    foreach (var res in response.Events)
                    {
                        viewModel.Add(_mapper.Map<GetEventsQueryViewModel>(res));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }

            return viewModel;
        }
    }
}
