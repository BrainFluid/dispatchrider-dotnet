using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DispatchRider.AspNetCore
{
    public interface IDispatchRiderService
    {
        Task HandleContextException(HttpContext context, Exception ex);
        Task HandleContextException(HttpContext context, string requestBody, Exception ex);
    }
}