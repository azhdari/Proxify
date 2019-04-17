using System.Threading.Tasks;

namespace Mohmd.AspNetCore.Proxify
{
    public interface IInterceptor
    {
        int Priority { get; }

        Task Intercept(IInvocation invocation);
    }
}
