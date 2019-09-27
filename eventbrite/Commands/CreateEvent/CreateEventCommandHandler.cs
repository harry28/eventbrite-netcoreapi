using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using eventbrite.Helpers;
using eventbrite.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eventbrite.Commands.CreateEvent
{
    public class CreateEventCommandHandler:IRequestHandler<CreateEventCommand, CreateEventCommandViewModel>
    {
        private readonly IMediator _mediator;
        private ILogger<CreateEventCommandHandler> _logger;
        private readonly IEventbriteService _eventbriteService;
        private IMapper _mapper;

        public CreateEventCommandHandler(IMediator mediator, ILogger<CreateEventCommandHandler> logger,
            IEventbriteService eventbriteService, IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _eventbriteService = eventbriteService;
            _mapper = mapper;
        }

        public async Task<CreateEventCommandViewModel> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            CreateEventCommandViewModel viewModel = new CreateEventCommandViewModel();
            try
            {
                var response = await _eventbriteService.CreateEvent(request);
                viewModel = _mapper.Map<CreateEventCommandViewModel>(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }

            return viewModel;
        }
    }
}
