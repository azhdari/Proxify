using System;
using System.Reflection;

namespace Mohmd.AspNetCore.Proxify.Internal
{
    public class Invocation : IInvocation
    {
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

        public void Proceed()
        {
            Proceeded?.Invoke(this, EventArgs.Empty);
        }

        public void SetReturnValue(object value)
        {
            ReturnValue = value;
        }

        #endregion
    }
}
