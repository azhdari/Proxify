namespace Mohmd.AspNetCore.Proxify
{
    public interface IInterceptor
    {
        int Priority { get; }

        void Intercept(IInvocation invocation);

        void InvokeBefore(IInvocation invocation);

        void InvokeAfter(IInvocation invocation);

        void InvokeOnException(IInvocation invocation);
    }
}
