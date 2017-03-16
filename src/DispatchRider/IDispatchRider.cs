using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DispatchRider.Request;

namespace DispatchRider
{
    public interface IDispatchRider
    {
        Task<DispatchRiderEvent> Dispatch(Exception ex);
        Task<DispatchRiderEvent> Dispatch(Exception ex, string httpMethod, string route, IDictionary<string,object> requestParams);
        Task<DispatchRiderEvent> Dispatch(DispatchRiderEvent evt);
    }
}