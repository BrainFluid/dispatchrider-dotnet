using System;
using System.Collections;
using System.Collections.Generic;

namespace DispatchRider.Request
{
    public class ExceptionRequest : ExceptionModel
    {
        public ExceptionRequest(Exception exception) : base(exception)
        {
            Exceptions = new List<ExceptionModel> {
                new ExceptionModel(exception)
            };
        }
        public string Route { get; set; }
        public string HttpMethod { get; set; }
        public IEnumerable<ExceptionModel> Exceptions { get; set; }
        public IDictionary<string, object> RequestParams { get; set; }

        public void AddException(Exception innerException)
        {
            ((List<ExceptionModel>)Exceptions).Add(new ExceptionModel(innerException));
        }
    }
}