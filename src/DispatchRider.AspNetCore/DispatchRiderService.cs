using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using DispatchRider;

namespace DispatchRider.AspNetCore
{
    public class DispatchRiderService : IDispatchRiderService
    {
        private readonly IDispatchRiderClient _dispatchRider;
        private readonly DispatchRiderMiddlewareOptions _options;

        public DispatchRiderService(IOptions<DispatchRiderOptions> options, IOptions<DispatchRiderMiddlewareOptions> middlewareOptions)
            : this(options.Value, middlewareOptions.Value) { }

        public DispatchRiderService(DispatchRiderOptions options, DispatchRiderMiddlewareOptions middlewareOptions)
        {
            _dispatchRider = new DispatchRider(options);
            _options = middlewareOptions;
        }

        public void HandleException(Exception ex)
        {
            Task.Run(() => _dispatchRider.Dispatch(ex));
        }

        public Task HandleContextException(HttpContext context, Exception ex)
        {
            return HandleContextException(context, ParseStream(context.Request.Body), ex, new Dictionary<string, object>());
        }

        public Task HandleContextException(HttpContext context, string requestBody, Exception ex)
        {
            return HandleContextException(context, requestBody, ex, new Dictionary<string, object>() );
        }

        public async Task HandleContextException(HttpContext context, string requestBody, Exception ex, IDictionary<string,object> requestParams)
        {
            try
            {
                if ( null != _options.ContextExceptionFilter ) {
                    _options.ContextExceptionFilter.HandleContextException(context);
                }
                await  _dispatchRider.Dispatch(ex, context.Request.Method, context.Request.Path.ToUriComponent(), requestParams);
            }
            catch
            {
                // _logger.LogError("An exception was thrown attempting to execute the error handler.", ex2);
                throw;
            }
            finally
            {
                // Free any resources
            }
        }

        public string ParseStream(Stream stream) {
            if ( null == stream ) {
                return string.Empty;
            }

            if ( stream.CanSeek ) {
                stream.Position = 0L;
            }

            var streamReader = new StreamReader(stream);
            var result = streamReader.ReadToEnd();

            return result;
        }
    }
}