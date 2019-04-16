using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Mohmd.AspNetCore.Proxify
{
    public interface IInvocation
    {
        object DecoratedObject { get; }

        MethodInfo Method { get; }

        object[] Arguments { get; }

        Exception Exception { get; }

        object ReturnValue { get; }

        void Proceed();
    }
}
