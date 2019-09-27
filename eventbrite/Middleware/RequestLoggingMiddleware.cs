using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace eventbrite.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private TelemetryClient TelemetryClient { get; set; }
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger, TelemetryClient telemetryClient)
        {
            _next = next;
            _logger = logger;
            TelemetryClient = telemetryClient;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var requestBody = new StreamReader(context.Request.Body).ReadToEnd();

                var bodyStream = context.Response.Body;
                var responseBodyStream = new MemoryStream();
                context.Response.Body = responseBodyStream;

                if (context.Request.Path != null && context.Request.Path != "/"
                                                 && !context.Request.Path.ToString().Contains(".ico")
                                                 && !context.Request.Path.ToString().Contains(".css")
                                                 && !context.Request.Path.ToString().Contains(".html")
                                                 && !context.Request.Path.ToString().Contains(".js")
                                                 && !context.Request.Path.ToString().Contains("images")
                                                 && !context.Request.Path.ToString().Contains("fonts")
                                                 && !context.Request.Path.ToString().Contains("font-awesome")
                                                 && !context.Request.Path.ToString().Contains(".well-known"))
                {
                    var logTemplate = @" 
                            Client IP: {clientIP} 
                            Request path: {requestPath} 
                            Request content type: {requestContentType} 
                            Request content length: {requestContentLength} 
                            RequestBody: {requestbody}";

                    TelemetryClient.TrackTrace(
                                                requestBody,
                                                SeverityLevel.Information,
                                                new Dictionary<string, string> {
                                                { "RequestType", "HTTP Request Logging" }
                                                }
                                               );

                    _logger.LogInformation(logTemplate,
                        context.Connection.RemoteIpAddress.ToString(),
                        context.Request.Path,
                        context.Request.ContentType,
                        context.Request.ContentLength,
                        requestBody);
                }
                context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));

                await _next.Invoke(context);

                responseBodyStream.Seek(0, SeekOrigin.Begin);
                var responseBody = new StreamReader(responseBodyStream).ReadToEnd();

                if (!string.IsNullOrWhiteSpace(responseBody) && context.Request.Path != null
                                                             && context.Request.Path != "/"
                                                             && !context.Request.Path.ToString().Contains(".ico")
                                                             && !context.Request.Path.ToString().Contains(".css")
                                                             && !context.Request.Path.ToString().Contains(".html")
                                                             && !context.Request.Path.ToString().Contains(".js")
                                                             && !context.Request.Path.ToString().Contains("images")
                                                             && !context.Request.Path.ToString().Contains("fonts")
                                                             && !context.Request.Path.ToString().Contains("font-awesome")
                                                             && !context.Request.Path.ToString().Contains(".well-known"))
                {
                    var logResponseTemplate = @" 
                            Client IP: {clientIP} 
                            Response content type: {responseContentType} 
                            Response content length: {responseContentLength} 
                            ResponseBody: {responsebody}";

                    TelemetryClient.TrackTrace(
                                                responseBody,
                                                SeverityLevel.Information,
                                                new Dictionary<string, string> {
                                                { "RequestType", "HTTP Response Logging" }
                                                }
                                               );

                    _logger.LogInformation(logResponseTemplate,
                        context.Connection.RemoteIpAddress.ToString(),
                        context.Response.ContentType,
                        context.Response.ContentLength,
                        responseBody);
                }
                if (context.Response.StatusCode != 204 && !string.IsNullOrWhiteSpace(responseBody))
                {
                    responseBodyStream.Seek(0, SeekOrigin.Begin);
                    await responseBodyStream.CopyToAsync(bodyStream);
                    context.Response.Body = bodyStream;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + Environment.NewLine + ex.StackTrace);
                TelemetryClient.TrackException(ex);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
