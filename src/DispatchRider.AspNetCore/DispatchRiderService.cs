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
        {
            _dispatchRider = new DispatchRider(options.Value);
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
                var requestParams = new Dictionary<string, object>();
                if ( context.Request.HasFormContentType ) {
                    foreach ( var key in context.Request.Form.Keys ) {
                        requestParams.Add(key, context.Request.Form[key]);
                    }
                } else {
                    foreach ( var key in context.Request.Query.Keys ) {
                        requestParams.Add(key, context.Request.Query[key]);
                    }
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
            if ( stream.CanSeek ) {
                stream.Position = 0L;
            }
            using ( var streamReader = new StreamReader(stream) ) {
                return streamReader.ReadToEnd();
            }
        }
    }
}