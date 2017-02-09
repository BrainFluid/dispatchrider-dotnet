using System;
using Microsoft.Extensions.DependencyInjection;

namespace DispatchRider.AspNetCore
{
    public class DispatchRiderServicesBuilder
    {
        private readonly IServiceCollection _serviceCollection;

        public DispatchRiderServicesBuilder(IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            _serviceCollection = serviceCollection;
        }

        public virtual IServiceCollection ServiceCollection
        {
            get { return _serviceCollection; }
        }
    }
}