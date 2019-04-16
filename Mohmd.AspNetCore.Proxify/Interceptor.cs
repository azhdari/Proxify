using System;
using System.Reflection;

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

        public void Intercept(IInvocation invocation)
        {
            throw new NotImplementedException();
        }
    }
}
