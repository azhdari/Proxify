using Microsoft.Extensions.Logging;

namespace Mohmd.AspNetCore.Proxify.Exmaple.Insterceptors
{
    public class LoggingInterceptor : IInterceptor
    {
        private readonly ILogger _logger;

        public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
        {
            _logger = logger;
        }

        public int Priority => 3;

        public void InvokeBefore(IInvocation invocation)
        {
            _logger.LogInformation($"### Before Method {invocation.Method.Name}");
        }

        public void InvokeAfter(IInvocation invocation)
        {
            _logger.LogInformation($"### After Method {invocation.Method.Name}");
        }

        public void InvokeOnException(IInvocation invocation)
        {
            _logger.LogError(invocation.Exception, $"### Error Method {invocation.Method.Name}");
        }
    }
}
