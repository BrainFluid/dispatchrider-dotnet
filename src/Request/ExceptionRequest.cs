using System.Collections;
using System.Collections.Generic;

namespace DispatchRider.Request
{
    public class ExceptionRequest
    {
        public string ExceptionType { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Route { get; set; }
        public string HttpMethod { get; set; }
        public IDictionary ExceptionData { get; set; }
        public IDictionary<string, object> RequestParams { get; set; }
    }
}