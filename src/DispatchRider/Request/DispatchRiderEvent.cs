using System;
using System.Collections.Generic;

namespace DispatchRider.Request
{
    public class DispatchRiderEvent
    {
        public Exception Exception { get; set; }
        public string HttpMethod { get; set; }
        public string Route { get; set; }
        public IDictionary<string,object> RequestParams { get; set; }
    }
}