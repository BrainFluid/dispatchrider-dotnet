using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DispatchRider.Request;
using Newtonsoft.Json;

namespace DispatchRider
{
    public class DispatchRider : IDispatchRider
    {
        private readonly IDispatchRiderOptions _options;

        public DispatchRider(IDispatchRiderOptions options)
        {
            _options = options;
        }
        public Task<DispatchRiderEvent> Dispatch(Exception ex)
        {
            return Dispatch(ex, string.Empty, string.Empty, null);
        }

        public Task<DispatchRiderEvent> Dispatch(Exception ex, string httpMethod, string route, IDictionary<string,object> requestParams)
        {
            return Dispatch(new DispatchRiderEvent {
                Exception = ex,
                HttpMethod = httpMethod,
                Route = route,
                RequestParams = requestParams
            });
        }

        public async Task<DispatchRiderEvent> Dispatch(DispatchRiderEvent evt)
        {
            if ( evt.Exception is AggregateException ) {
                foreach ( var agrex in ((AggregateException)evt.Exception).Flatten().InnerExceptions ) {
                    await Dispatch(agrex, evt.HttpMethod, evt.Route, evt.RequestParams);
                }
                return evt;
            }
            var endpoint = new Uri(_options.BaseUri, "/1.0/record/exception");
            using (var client = new HttpClient())
            {
                var req = new ExceptionRequest();
                req.ExceptionType = evt.Exception.GetType().ToString();
                req.Message = evt.Exception.Message;
                req.StackTrace = evt.Exception.StackTrace;
                req.HttpMethod = evt.HttpMethod;
                req.ExceptionData = evt.Exception.Data;
                req.Route = evt.Route;
                req.RequestParams = evt.RequestParams;
                var json = JsonConvert.SerializeObject(req);
                using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    var request = await client.PostAsync(endpoint, content);
                }
            }
            return evt;
        }
    }
}