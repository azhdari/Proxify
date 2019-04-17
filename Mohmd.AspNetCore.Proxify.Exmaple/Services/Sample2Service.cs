using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mohmd.AspNetCore.Proxify.Exmaple.Services
{
    public class Sample2Service
    {
        public Task<string> GetName()
        {
            throw new Exception("TEST TEST Sample2");
            return Task.FromResult("Sample2");
        }
    }
}
