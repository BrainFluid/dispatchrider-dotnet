using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DispatchRider
{
    public interface IDispatchRider
    {
        Task Dispatch(Exception ex);
        Task Dispatch(Exception ex, string httpMethod, string route, IDictionary<string,object> requestParams);
    }
}