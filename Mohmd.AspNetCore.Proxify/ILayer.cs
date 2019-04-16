using System;
using System.Reflection;

namespace Mohmd.AspNetCore.Proxify
{
    public interface ILayer
    {
        void InvokeBefore(MethodInfo methodInfo, object[] args);

        void InvokeAfter(MethodInfo methodInfo, object[] args, object result);

        void InvokeOnException(MethodInfo methodInfo, Exception exception);
    }
}
