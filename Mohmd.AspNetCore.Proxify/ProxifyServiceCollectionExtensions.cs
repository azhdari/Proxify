using Microsoft.Extensions.Logging;
using Mohmd.AspNetCore.Proxify;
using Mohmd.AspNetCore.Proxify.Internal;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ProxifyServiceCollectionExtensions
    {
        public static IProxifyBuilder AddProxify(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return new ProxifyBuilder(services);
        }

        public static IServiceCollection AddTransientProxyService<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddTransient(AddProxyService<TService, TImplementation>);
            return services;
        }

        public static IServiceCollection AddScopedProxyService<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddScoped(AddProxyService<TService, TImplementation>);
            return services;
        }

        public static IServiceCollection AddSingletonProxyService<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddSingleton(AddProxyService<TService, TImplementation>);
            return services;
        }

        private static TService AddProxyService<TService, TImplementation>(IServiceProvider serviceProvider)
            where TService : class
            where TImplementation : class, TService
        {
            var instance = ActivatorUtilities.CreateInstance<TImplementation>(serviceProvider);
            var logger = serviceProvider.GetService<ILogger<TService>>();
            var result = DispatchProxy<TService>.Create(instance, logger, serviceProvider);
            return result;
        }
    }
}
