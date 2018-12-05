using System;
using System.Collections;
using System.Collections.Generic;

namespace DispatchRider.Request
{
    public class ExceptionRequest : ExceptionModel
    {
        public ExceptionRequest() : this(null) { }
        public ExceptionRequest(Exception exception) : base(exception)
        {
            RequestParams = new Dictionary<string, object>();
            Exceptions = new List<ExceptionModel>();
            if ( null != exception ) {
                Exceptions.Add(new ExceptionModel(exception));
            };
        }
        public string Route { get; set; }
        public string HttpMethod { get; set; }
        public UserInfo UserInfo { get; set; }
        public IList<ExceptionModel> Exceptions { get; set; }
        public IDictionary<string, object> RequestParams { get; set; }

        public void AddException(Exception innerException)
        {
            Exceptions.Add(new ExceptionModel(innerException));
        }
    }
}