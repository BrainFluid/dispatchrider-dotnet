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
        public Task Dispatch(Exception ex)
        {
            return Dispatch(ex, string.Empty, string.Empty, null);
        }

        public async Task Dispatch(Exception ex, string httpMethod, string route, IDictionary<string,object> requestParams)
        {
            if ( ex is AggregateException ) {
                foreach ( var agrex in ((AggregateException)ex).Flatten().InnerExceptions ) {
                    await Dispatch(agrex, httpMethod, route, requestParams);
                }
                return;
            }
            var endpoint = new Uri(_options.BaseUri, "/1.0/record/exception");
            using (var client = new HttpClient())
            {
                var req = new ExceptionRequest();
                req.ExceptionType = ex.GetType().ToString();
                req.Message = ex.Message;
                req.StackTrace = ex.StackTrace;
                req.HttpMethod = httpMethod;
                req.ExceptionData = ex.Data;
                req.Route = route;
                req.RequestParams = requestParams;
                var json = JsonConvert.SerializeObject(req);
                using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    var request = await client.PostAsync(endpoint, content);
                }
            }
        }
    }
    /*
    

    public class DispatchRider
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DispatchRider InnerException { get; set; }
        public IDictionary<string, string> RequestArguments { get; set; }
    }
    */
}