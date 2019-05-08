using System;
using System.Threading.Tasks;

namespace Mohmd.AspNetCore.Proxify
{
    public abstract class Interceptor : IInterceptor
    {
        public abstract int Priority { get; }

        public Task Intercept(IInvocation invocation)
        {
            throw new NotImplementedException();
        }
    }
}
