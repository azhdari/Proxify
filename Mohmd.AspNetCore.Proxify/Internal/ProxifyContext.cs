using System;
using System.Collections.Generic;

namespace Mohmd.AspNetCore.Proxify.Internal
{
    internal static class ProxifyContext
    {
        public static List<Type> InterceptorTypes { get; } = new List<Type>();
    }
}
