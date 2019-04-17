using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mohmd.AspNetCore.Proxify
{
    public interface IInvocation
    {
        object DecoratedObject { get; }

        MethodInfo Method { get; }

        object[] Arguments { get; }

        object ReturnValue { get; }

        Task Proceed();
    }
}
