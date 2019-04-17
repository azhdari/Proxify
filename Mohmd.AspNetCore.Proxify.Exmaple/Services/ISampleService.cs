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

        Task<int> SumAsync(int a, int b);

        Task<string> GetName();

        Task SetName(string name);
    }
}
