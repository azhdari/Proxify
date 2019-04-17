using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Mohmd.AspNetCore.Proxify.Internal
{
    public class Invocation : IInvocation
    {
        #region Fields

        private TaskCompletionSource<int> _taskCompletionSource;

        #endregion

        #region Ctors

        public Invocation(object decoratedObject, MethodInfo method, object[] arguments)
            : this(decoratedObject, method, arguments, null, null)
        {
        }

        public Invocation(object decoratedObject, MethodInfo method, object[] arguments, Exception exception)
            : this(decoratedObject, method, arguments, exception, null)
        {
        }

        public Invocation(object decoratedObject, MethodInfo method, object[] arguments, object result)
            : this(decoratedObject, method, arguments, null, result)
        {
        }

        public Invocation(object decoratedObject, MethodInfo method, object[] arguments, Exception exception, object result)
        {
            DecoratedObject = decoratedObject;
            Method = method;
            Arguments = arguments;
            Exception = exception;
            ReturnValue = result;
        }

        #endregion

        #region Events

        public event EventHandler Proceeded;

        #endregion

        #region Properties

        public object DecoratedObject { get; private set; }

        public MethodInfo Method { get; private set; }

        public object[] Arguments { get; private set; }

        public Exception Exception { get; private set; }

        public object ReturnValue { get; private set; }

        #endregion

        #region Methods

        public Task Proceed()
        {
            // this line must be before invoking event, SetReturnValue() might be called before that. 
            _taskCompletionSource = new TaskCompletionSource<int>();

            Proceeded?.Invoke(this, EventArgs.Empty);
            return _taskCompletionSource.Task;
        }

        public void SetReturnValue(object value)
        {
            ReturnValue = value;
            _taskCompletionSource?.SetResult(0);
        }

        public void SetException(Exception exception)
        {
            this.Exception = exception;
        }

        #endregion
    }
}
