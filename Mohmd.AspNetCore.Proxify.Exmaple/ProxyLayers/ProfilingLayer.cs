using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Mohmd.AspNetCore.Proxify.Exmaple.ProxyLayers
{
    public class ProfilingLayer : BaseLayer
    {
        private Stopwatch _stopwatch = new Stopwatch();
        private readonly ILogger _logger;
        private readonly Guid _guid = Guid.NewGuid();

        public ProfilingLayer(ILogger<ProfilingLayer> logger)
        {
            _logger = logger;
        }

        public override void InvokeBefore(MethodInfo methodInfo, object[] args)
        {
            _logger.LogWarning($"### Before Method {methodInfo.Name} id {_guid}");
            _stopwatch.Start();
        }

        public override void InvokeAfter(MethodInfo methodInfo, object[] args, object result)
        {
            _stopwatch.Stop();

            _logger.LogWarning($"### After Method {methodInfo.Name} id {_guid}");
            _logger.LogWarning($"@@@ Invoking method {methodInfo.Name} took {_stopwatch.Elapsed.ToString()} with result of {result}");
        }
    }
}
