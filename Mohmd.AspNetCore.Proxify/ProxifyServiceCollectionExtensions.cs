using Microsoft.Extensions.Logging;
using Mohmd.AspNetCore.Proxify;
using Mohmd.AspNetCore.Proxify.Internal;
using System;
using System.Linq;
using System.Reflection;

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
            services.AddTransient(GetProxyService<TService, TImplementation>);
            return services;
        }

        public static IServiceCollection AddTransientProxyService(this IServiceCollection services, Type interfaceType, Type implementType)
        {
            typeof(ProxifyServiceCollectionExtensions)
                .GetMethods()
                .Where(x => x.Name == "AddTransientProxyService" && x.ContainsGenericParameters)
                .FirstOrDefault()?
                .MakeGenericMethod(interfaceType, implementType)
                .Invoke(null, new object[] { services });

            return services;
        }

        public static IServiceCollection AddScopedProxyService<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddScoped(GetProxyService<TService, TImplementation>);
            return services;
        }

        public static IServiceCollection AddScopedProxyService(this IServiceCollection services, Type interfaceType, Type implementType)
        {
            typeof(ProxifyServiceCollectionExtensions)
                .GetMethods()
                .Where(x => x.Name == "AddScopedProxyService" && x.ContainsGenericParameters)
                .FirstOrDefault()?
                .MakeGenericMethod(interfaceType, implementType)
                .Invoke(null, new object[] { services });

            return services;
        }

        public static IServiceCollection AddSingletonProxyService<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddSingleton(GetProxyService<TService, TImplementation>);
            return services;
        }

        public static IServiceCollection AddSingletonProxyService(this IServiceCollection services, Type interfaceType, Type implementType)
        {
            typeof(ProxifyServiceCollectionExtensions)
                .GetMethods()
                .Where(x => x.Name == "AddSingletonProxyService" && x.ContainsGenericParameters)
                .FirstOrDefault()?
                .MakeGenericMethod(interfaceType, implementType)
                .Invoke(null, new object[] { services });

            return services;
        }

        public static TService GetProxyService<TService, TImplementation>(IServiceProvider serviceProvider)
            where TService : class
            where TImplementation : class, TService
        {
            var instance = ActivatorUtilities.CreateInstance<TImplementation>(serviceProvider);
            var result = DispatchProxy<TService>.Create(instance, serviceProvider);
            return result;
        }
    }
}
