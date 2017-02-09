using System;

namespace DispatchRider
{
    public interface IDispatchRiderOptions
    {
        Uri BaseUri { get; set; }
        string ApiKey { get; set; }
    }
}