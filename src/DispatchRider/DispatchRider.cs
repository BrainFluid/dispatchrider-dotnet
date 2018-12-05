using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DispatchRider.Request;
using Newtonsoft.Json;

namespace DispatchRider
{
    public class DispatchRider : IDispatchRiderClient
    {
        private readonly IDispatchRiderOptions _options;

        public DispatchRider(IDispatchRiderOptions options)
        {
            _options = options;
            UserInfo = new UserInfo();
        }
        public UserInfo UserInfo { get; set; }
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
                if ( !string.IsNullOrEmpty(_options.ApiKey)) {
                    var authArray = Encoding.UTF8.GetBytes("APIKEYUSER:" + _options.ApiKey);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authArray));
                }
                var req = new ExceptionRequest(evt.Exception);
                req.HttpMethod = evt.HttpMethod;
                req.Route = evt.Route;
                req.RequestParams = evt.RequestParams;
                var innerException = evt.Exception.InnerException;
                while ( null != innerException ) {
                    req.AddException(innerException);
                    innerException = innerException.InnerException;
                }
                var json = JsonConvert.SerializeObject(req);
                System.Console.WriteLine("Preparing to send payload");
                System.Console.WriteLine(json);
                using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    var request = await client.PostAsync(endpoint, content);
                }
            }
            return evt;
        }
    }
}