using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Mohmd.AspNetCore.Proxify.Exmaple.ProxyLayers
{
    public class ProfilingInsterceptor : IInterceptor
    {
        private Stopwatch _stopwatch = new Stopwatch();
        private readonly ILogger _logger;

        public int Priority => 2;

        public ProfilingInsterceptor(ILogger<ProfilingInsterceptor> logger)
        {
            _logger = logger;
        }

        public void InvokeBefore(IInvocation invocation)
        {
            _stopwatch.Start();
        }

        public void InvokeAfter(IInvocation invocation)
        {
            _stopwatch.Stop();

            _logger.LogWarning($"@@@ Invoking method {invocation.Method.Name} took {_stopwatch.Elapsed.ToString()} with result of {invocation.Result}");
        }

        public void InvokeOnException(IInvocation invocation)
        {
            
        }
    }
}
