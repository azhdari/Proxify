using System;
using System.Reflection;

namespace Mohmd.AspNetCore.Proxify
{
    public abstract class BaseLayer : ILayer
    {
        public virtual void InvokeBefore(MethodInfo methodInfo, object[] args)
        {
            throw new NotImplementedException();
        }

        public virtual void InvokeAfter(MethodInfo methodInfo, object[] args, object result)
        {
            throw new NotImplementedException();
        }

        public virtual void InvokeOnException(MethodInfo methodInfo, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
