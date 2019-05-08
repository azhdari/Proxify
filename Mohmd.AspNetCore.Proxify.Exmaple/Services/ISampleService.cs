using Mohmd.AspNetCore.Proxify.Attributes;
using Mohmd.AspNetCore.Proxify.Exmaple.Insterceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mohmd.AspNetCore.Proxify.Exmaple.Services
{
    public interface ISampleService
    {
        void VoidMethod();

        int Sum(int a, int b);

        Guid? GetSampleGuid();

        //[ApplyInterceptors(typeof(ProfilingInsterceptor))]
        Task<int> SumAsync(int a, int b);

        [IgnoreInterceptors(typeof(ProfilingInsterceptor))]
        Task<string> GetName();

        Task SetName(string name);
    }
}
