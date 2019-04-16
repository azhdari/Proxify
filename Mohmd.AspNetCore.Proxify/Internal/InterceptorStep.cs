namespace Mohmd.AspNetCore.Proxify.Internal
{
    internal enum InterceptorStep : int
    {
        Before = 0,
        After = 10,
        OnException = 20,
    }
}
