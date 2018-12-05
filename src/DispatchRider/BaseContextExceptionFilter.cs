using System.Collections.Generic;

namespace DispatchRider
{
    public class BaseContextExceptionFilter
    {
        public void HandleContextException(IHttpContext context)
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
        }
    }
}