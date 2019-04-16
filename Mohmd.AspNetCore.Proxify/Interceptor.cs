using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Mohmd.AspNetCore.Proxify
{
    public abstract class Interceptor : IInterceptor
    {
        public abstract int Priority { get; }

        public virtual void InvokeBefore(IInvocation invocation)
        {
            throw new NotImplementedException();
        }

        public virtual void InvokeAfter(IInvocation invocation)
        {
            throw new NotImplementedException();
        }

        public virtual void InvokeOnException(IInvocation invocation)
        {
            throw new NotImplementedException();
        }

        public Task Intercept(IInvocation invocation)
        {
            throw new NotImplementedException();
        }
    }
}
