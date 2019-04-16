using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Mohmd.AspNetCore.Proxify
{
    public interface IProxifyBuilder
    {
        IProxifyBuilder AddAssemblyByType(Type type);

        IProxifyBuilder AddAssemblyByType<T>();

        IProxifyBuilder AddAssembly(params Assembly[] assemblies);

        IServiceCollection Build();
    }
}
