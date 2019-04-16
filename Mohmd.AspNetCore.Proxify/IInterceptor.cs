using System.Threading.Tasks;

namespace Mohmd.AspNetCore.Proxify
{
    public interface IInterceptor
    {
        int Priority { get; }

        Task Intercept(IInvocation invocation);

        void InvokeBefore(IInvocation invocation);

        void InvokeAfter(IInvocation invocation);

        void InvokeOnException(IInvocation invocation);
    }
}
