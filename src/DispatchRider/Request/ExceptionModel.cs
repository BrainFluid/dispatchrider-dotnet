using System;
using System.Collections;

namespace DispatchRider.Request
{
    public class ExceptionModel
    {
        public ExceptionModel()
        {
        }
        public ExceptionModel(Exception exception)
        {
            if ( null != exception ) {
                ExceptionType = exception.GetType().ToString();
                Message = exception.Message;
                StackTrace = exception.StackTrace;
                ExceptionData = exception.Data;
            }
        }
        public string ExceptionType { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public IDictionary ExceptionData { get; set; }
    }
}