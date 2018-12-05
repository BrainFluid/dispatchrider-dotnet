using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DispatchRider.AspNetCore
{
    public class DispatchRiderService : IDispatchRiderService
    {
        private readonly IDispatchRider _dispatchRider;

        public DispatchRiderService(IOptions<DispatchRiderOptions> options)
            : this(options.Value) { }

        public DispatchRiderService(DispatchRiderOptions options)
        {
            _dispatchRider = new DispatchRider(options);
            _dispatchRider.ContextExceptionFilter = new BaseContextExceptionFilter();
        }

        public void HandleException(Exception ex)
        {
            Task.Run(() => _dispatchRider.Dispatch(ex));
        }

        public Task HandleContextException(HttpContext context, Exception ex)
        {
            return HandleContextException(context, ParseStream(context.Request.Body), ex);
        }

        public async Task HandleContextException(HttpContext context, string requestBody, Exception ex)
        {
            try
            {
                if ( null != _dispatchRider.ContextExceptionFilter ) {
                    _dispatchRider.ContextExceptionFilter.HandleContextException();
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