using System;

namespace DispatchRider
{
    public class DispatchRiderOptions : IDispatchRiderOptions
    {
        public DispatchRiderOptions()
        {
            BaseUri = new Uri("https://dispatchrider.azurewebsites.net/");
        }
        public Uri BaseUri { get; set; }
        public string ApiKey { get; set; }
    }
}