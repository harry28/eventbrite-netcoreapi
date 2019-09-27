using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using eventbrite.Commands.CreateEvent;
using eventbrite.CustomExceptions;
using eventbrite.Queries.GetEventById;
using eventbrite.Queries.GetEvents;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling;

namespace eventbrite.Controllers
{
    [Produces("application/json")]
    [Route("api/Eventbrite/[action]")]
    [ApiController]
    public class EventbriteController : Controller
    {
        private readonly ILogger<EventbriteController> _logger;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        public EventbriteController(ILogger<EventbriteController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
        }

        ///  <summary>
        ///  Get Events.
        ///  </summary>
        /// <returns>Events Information</returns>
        /// <response code = "200"> Returns events information</response>
        /// <response code = "400"> If there is an error please check </response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEvents()
        {
            using (MiniProfiler.Current.Step("Get Events"))
            {
                GetEventsQuery query=new GetEventsQuery();
                List<GetEventsQueryViewModel> model = await _mediator.Send(query);
                if (model != null)
                    return Ok(model);
                throw new NotFoundCustomException("No data found", $"Please check your parameters : ");
            }
        }

        ///  <summary>
        ///  Get Event by Id
        ///  </summary>
        ///  <remarks>
        ///  Sample request:
        /// 
        ///      GET /EventId
        ///      {
        ///         "EventId": "74445312935",        
        ///      }
        /// 
        ///  </remarks>
        /// <returns>Event Information</returns>
        /// <response code = "200"> Returns Event information for the given Event Id</response>
        /// <response code = "400"> If there is an error please check in application insights</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEventById([FromQuery]GetEventByIdQuery query)
        {
            using (MiniProfiler.Current.Step("Get Event by Id"))
            {
                GetEventByIdQueryViewModel model = await _mediator.Send(query);
                if (model != null)
                    return Ok(model);
                throw new NotFoundCustomException("No data found", $"Please check your parameters id: {query.EventId}");
            }
        }


        ///  <summary>
        ///  Create Events.
        ///  </summary>
        /// <returns>Create Event Information</returns>
        /// <response code = "201"> Returns event information</response>
        /// <response code = "400"> If there is an error please check </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateEvent([FromBody]CreateEventCommand command)
        {
            using (MiniProfiler.Current.Step("Create Event"))
            {
                var model = await _mediator.Send(command);
                if (model != null)
                    return CreatedAtAction(
                        nameof(GetEventById), new { id = model.id }, model); ;
                return BadRequest(ModelState);
            }
        }
    }
}