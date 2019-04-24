using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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

        public async Task Intercept(IInvocation invocation)
        {
            try
            {
                _logger.LogInformation($"# (Intercept) Before {invocation.Method.Name}.");
                await invocation.Proceed();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"# (Intercept) Error {invocation.Method.Name}.");
                throw ex;
            }
            finally
            {
                _logger.LogInformation($"# (Intercept) After {invocation.Method.Name} = `{invocation.ReturnValue}`.");
            }
        }
    }
}
