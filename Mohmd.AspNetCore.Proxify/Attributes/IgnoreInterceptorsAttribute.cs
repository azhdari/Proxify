using System;
using System.Collections.Generic;

namespace Mohmd.AspNetCore.Proxify.Attributes
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IgnoreInterceptorsAttribute : Attribute
    {
        public IgnoreInterceptorsAttribute(params Type[] interceptorTypes)
        {
            Interceptors = interceptorTypes;
        }

        public IReadOnlyList<Type> Interceptors { get; private set; }
    }
}
