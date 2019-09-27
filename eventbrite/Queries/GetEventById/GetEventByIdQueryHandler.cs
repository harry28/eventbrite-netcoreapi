using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using eventbrite.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eventbrite.Queries.GetEventById
{
    public class GetEventByIdQueryHandler: IRequestHandler<GetEventByIdQuery, GetEventByIdQueryViewModel>
    {
        private readonly IMediator _mediator;
        private ILogger<GetEventByIdQueryHandler> _logger;
        private readonly IEventbriteService _eventbriteService;
        private IMapper _mapper;

        public GetEventByIdQueryHandler()
        {

        }

        public GetEventByIdQueryHandler(IMediator mediator, ILogger<GetEventByIdQueryHandler> logger, IEventbriteService eventbriteService,IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _eventbriteService = eventbriteService;
            _mapper = mapper;
        }

        public async Task<GetEventByIdQueryViewModel> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            GetEventByIdQueryViewModel viewModel=new GetEventByIdQueryViewModel();
            try
            {
                var response = await _eventbriteService.GetEventById(request.EventId);
                viewModel = _mapper.Map<GetEventByIdQueryViewModel>(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }

            return viewModel;
        }
    }
}
