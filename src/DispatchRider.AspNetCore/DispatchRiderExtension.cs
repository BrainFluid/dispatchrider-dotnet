using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DispatchRider.AspNetCore
{
    public static class DispatchRiderExtension
    {
        static public IApplicationBuilder UseDispatchRider(this IApplicationBuilder app)
        {
            EnableRewind(app);
            return app.UseMiddleware<DispatchRiderMiddleware>();
        }

        static private void EnableRewind(IApplicationBuilder app)
        {
            app.Use(next => async context => {
                var initialBody = context.Request.Body;
                context.Request.EnableRewind();

                await next(context);
                return;
            });

        }
        static public DispatchRiderServicesBuilder AddDispatchRider(this IServiceCollection services, IConfiguration configuration)
        {
            return AddDispatchRider(services, configuration, o => new DispatchRiderMiddlewareOptions());
        }
        static public DispatchRiderServicesBuilder AddDispatchRider(this IServiceCollection services, IConfiguration configuration, Action<DispatchRiderMiddlewareOptions> configureOptions)
        {
            services.TryAddTransient<IDispatchRiderService, DispatchRiderService>();
            services.Configure<DispatchRiderOptions>(_ => configuration.GetSection("DispatchRider"));
            services.Configure<FormOptions>(s => {
                s.BufferBody = true;
            });
            services.Configure(configureOptions);

            return new DispatchRiderServicesBuilder(services);
        }
    }
}