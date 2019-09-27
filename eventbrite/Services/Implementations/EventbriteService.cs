using System;
using System.Net;
using System.Threading.Tasks;
using eventbrite.Commands.CreateEvent;
using eventbrite.Helpers;
using eventbrite.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace eventbrite.Services.Implementations
{
    public class EventbriteService:IEventbriteService
    {
        private readonly IOptions<EventBriteSettings> _eventbriteSettings;
        private readonly ILogger<EventbriteService> _logger;

        public EventbriteService(IOptions<EventBriteSettings> eventbriteSettings, ILogger<EventbriteService> logger)
        {
            _logger = logger;
            _eventbriteSettings = eventbriteSettings;
        }

        #region Queries
        /// <summary>
        /// Get Events
        /// </summary>
        /// <returns></returns>
        public async Task<EventbriteEvents> GetEvents()
        {
            EventbriteEvents model = new EventbriteEvents();

            try
            {
                //Get RestClient
                var client = GetRestClient(string.Empty);

                //Get Events
                IRestResponse response = await GetEvents(client);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogInformation("Error");
                }
                else
                {
                    model = JsonConvert.DeserializeObject<EventbriteEvents>(response.Content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }

            return model;
        }

        /// <summary>
        /// Get Event by Id
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<Event> GetEventById(string eventId)
        {
            Event model = new Event();

            try
            {
                //Get RestClient
                var client = GetRestClient(eventId);

                //Get Event
                IRestResponse response = await GetEvents(client);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogInformation("Error");
                }
                else
                {
                    model = JsonConvert.DeserializeObject<Event>(response.Content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }

            return model;
        }

        #endregion

        #region "Commands"

        public async Task<Event> CreateEvent(CreateEventCommand newEvent)
        {
            Event model = new Event();

            try
            {
                //Create RestClient
                var client = CreateRestClient(_eventbriteSettings.Value.OrganizationId);

                //Get Payload
                IRestResponse response = await CreateEvent(client, newEvent);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogInformation("Error");
                }
                else
                {
                    model = JsonConvert.DeserializeObject<Event>(response.Content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }

            return model;
        }

        #endregion

        #region "Get Event Connections"
        /// <summary>
        /// Get Events Client
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        RestClient GetRestClient(string eventId)
        {
            const string logInfo = "EventbriteService, GetRestClient";
            RestClient client = null;
            try
            {
                if (String.IsNullOrEmpty(eventId))
                {
                    client = new RestClient(
                        _eventbriteSettings.Value.GetEventsUrl
                        + "?token=" + _eventbriteSettings.Value.PrivateToken
                    );
                }
                else
                {
                    client = new RestClient(
                        _eventbriteSettings.Value.GetEventUrl
                        + eventId
                        + "?token=" + _eventbriteSettings.Value.PrivateToken
                    );
                }
            }
            catch (Exception ex)
            {
                string error = logInfo + " Error Message: " + ex.Message + Environment.NewLine + ex.StackTrace;
                _logger.LogInformation(error);
            }

            return client;
        }

        /// <summary>
        /// Get Events
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        async Task<IRestResponse> GetEvents(RestClient client)
        {
            const string logInfo = "EventbriteService, GetEvents";
            IRestResponse response = null;
            try
            {
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                response = await client.ExecuteGetTaskAsync(request);
            }
            catch (Exception ex)
            {
                string error = logInfo + " Error Message: " + ex.Message + Environment.NewLine + ex.StackTrace;
                _logger.LogInformation(error);
            }

            return response;
        }

        #endregion

        #region "Create Event Connections"

        /// <summary>
        /// Create Event Client
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        RestClient CreateRestClient(string organizationId)
        {
            const string logInfo = "EventbriteService, CreateRestClient";
            RestClient client = null;
            try
            {
                client = new RestClient(
                    _eventbriteSettings.Value.CreateUrl
                    + organizationId
                    + "/events/"
                    + "?token=" + _eventbriteSettings.Value.PrivateToken
                );

            }
            catch (Exception ex)
            {
                string error = logInfo + " Error Message: " + ex.Message + Environment.NewLine + ex.StackTrace;
                _logger.LogInformation(error);
            }

            return client;
        }

        /// <summary>
        /// Create Events
        /// </summary>
        /// <param name="client"></param>
        /// <param name="newEvent"></param>
        /// <returns></returns>
        async Task<IRestResponse> CreateEvent(RestClient client, CreateEventCommand newEvent)
        {
            const string logInfo = "EventbriteService, CreateEvent";
            IRestResponse response = null;
            try
            {
                var eventString = JsonConvert.SerializeObject(newEvent);
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddParameter("undefined", eventString, ParameterType.RequestBody);
                response = await client.ExecutePostTaskAsync(request);
            }
            catch (Exception ex)
            {
                string error = logInfo + " Error Message: " + ex.Message + Environment.NewLine + ex.StackTrace;
                _logger.LogInformation(error);
            }

            return response;
        }

        #endregion
    }
}
