using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohmd.AspNetCore.Proxify.Exmaple.Services
{
    public class InnerService
    {
        public void Method()
        {
            throw new Exception("This is an error from inner service");
        }
    }
}
