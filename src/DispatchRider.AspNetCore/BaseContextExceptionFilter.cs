using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace DispatchRider.AspNetCore
{
    public class BaseContextExceptionFilter
    {
        public virtual void ConfigureClient(IDispatchRiderClient client, HttpContext context)
        {

        }
        public virtual void HandleContextException(HttpContext context)
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