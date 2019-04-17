using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mohmd.AspNetCore.Proxify.Internal
{
    internal class ProxifyBuilder : IProxifyBuilder
    {
        private readonly IServiceCollection _services;
        private List<Assembly> _assemblyList = new List<Assembly>();

        public ProxifyBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IProxifyBuilder AddAssembly(params Assembly[] assemblies)
        {
            _assemblyList.AddRange(assemblies);
            return this;
        }

        public IProxifyBuilder AddAssemblyByType(Type type)
        {
            _assemblyList.Add(type.Assembly);
            return this;
        }

        public IProxifyBuilder AddAssemblyByType<T>()
        {
            return AddAssemblyByType(typeof(T));
        }

        public IServiceCollection Build()
        {
            Type[] baseTypes = new[] { typeof(IInterceptor), typeof(Interceptor) };
            Type[] interceptors = _assemblyList
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => baseTypes.Any(baseType => baseType.IsAssignableFrom(type) && type != baseType))
                .Distinct()
                .ToArray();

            ProxifyContext.InterceptorTypes.AddRange(interceptors);

            foreach (var type in interceptors)
            {
                _services.AddTransient(type);
            }

            return _services;
        }
    }
}
