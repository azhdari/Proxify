using Mohmd.AspNetCore.Proxify.Attributes;
using Mohmd.AspNetCore.Proxify.Exmaple.Insterceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mohmd.AspNetCore.Proxify.Exmaple.Services
{
    public class SampleService : ISampleService
    {
        private string _name = string.Empty;

        public void VoidMethod()
        {
        }

        public int Sum(int a, int b)
        {
            return a + b;
        }

        public async Task<int> SumAsync(int a, int b)
        {
            await Task.Delay(1000);
            new InnerService().Method();
            return a + b;
        }

        public Task<string> GetName()
        {
            return Task.FromResult(_name);
        }

        public Task SetName(string name)
        {
            _name = name;
            return Task.CompletedTask;
        }
    }
}
