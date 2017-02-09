using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DispatchRider.AspNetCore
{
    public class DispatchRiderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IDispatchRiderService _dispatchRider;

        public DispatchRiderMiddleware(RequestDelegate next, 
                                    ILoggerFactory loggerFactory,
                                    IDispatchRiderService dispatchService)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<DispatchRiderMiddleware>();
            _dispatchRider = dispatchService;
        }

        public async Task Invoke(HttpContext context)
        {
            string body = string.Empty;
            try
            {
                body = new StreamReader(context.Request.Body).ReadToEnd();
                byte[] requestData = Encoding.UTF8.GetBytes(body);
                context.Request.Body = new MemoryStream(requestData);

                await _next(context);
            }
            catch (Exception ex)
            {
                await _dispatchRider.HandleContextException(context, ex);
                throw; // Re-throw the original, we don't handle we only report
            }
        }
    }
}