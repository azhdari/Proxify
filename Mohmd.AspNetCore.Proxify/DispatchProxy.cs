using Mohmd.AspNetCore.Proxify.Internal;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mohmd.AspNetCore.Proxify
{
    public class DispatchProxy<T> : DispatchProxy
        where T : class
    {
        private T _decorated;
        private IServiceProvider _serviceProvider;
        private ConcurrentDictionary<string, IInterceptor[]> _interceptors = new ConcurrentDictionary<string, IInterceptor[]>();

        protected override object Invoke(MethodInfo methodInfo, object[] args)
        {
            if (methodInfo == null)
            {
                return null;
            }

            var interceptors = GetInterceptors(methodInfo)
                .OrderBy(x => x.Priority)
                .ToArray();

            int index = -1;

            Invocation invocation = new Invocation(_decorated, methodInfo, args);
            invocation.Proceeded += (sender, e) =>
            {
                Proceed();
            };

            object methodReturnValue = null;
            Task lastTask = null;

            void Final()
            {
                var result = methodInfo.Invoke(_decorated, args);
                methodReturnValue = result;

                if (result is Task resultTask)
                {
                    resultTask.ContinueWith(task =>
                    {
                        if (task.Exception == null)
                        {
                            object taskResult = null;

                            if (task.GetType().GetTypeInfo().IsGenericType)
                            {
                                taskResult = task
                                    .GetType()
                                    .GetTypeInfo()
                                    .GetProperties()
                                    .FirstOrDefault(p => p.Name == "Result")?
                                    .GetValue(task);
                            }

                            invocation.SetReturnValue(taskResult);
                        }
                    });
                }
                else
                {
                    invocation.SetReturnValue(result);
                }
            }

            void Proceed()
            {
                index += 1;
                int thisLoopIndex = index;
                var interceptor = interceptors.ElementAtOrDefault(index);

                if (interceptor != null)
                {
                    lastTask = interceptor.Intercept(invocation);
                    lastTask.ContinueWith(prev =>
                    {
                        if (prev.IsFaulted && invocation.Exception == null)
                        {
                            if (prev.Exception is AggregateException aggregateException)
                            {
                                invocation.SetException(aggregateException.InnerException);
                            }
                            else
                            {
                                invocation.SetException(prev.Exception);
                            }
                        }

                        if (thisLoopIndex == index)
                        {
                            // interceptor has not call Proceed()
                            Final();
                        }
                    });
                }
                else
                {
                    try
                    {
                        Final();
                    }
                    catch (Exception ex)
                    {
                        invocation.SetException(ex);
                        throw ex;
                    }
                }
            }

            if (interceptors.Any())
            {
                Proceed();
            }

            lastTask?.Wait();

            if (invocation.Exception != null)
            {
                throw invocation.Exception;
            }

            if (lastTask?.IsFaulted == true)
            {
                throw lastTask.Exception;
            }

            if (methodReturnValue == null && methodInfo.ReturnType != typeof(void))
            {
                throw new Exception("An exception happened");
            }

            return methodReturnValue;
        }

        public static T Create(T decorated, IServiceProvider serviceProvider)
        {
            object proxy = Create<T, DispatchProxy<T>>();
            ((DispatchProxy<T>)proxy).SetParameters(decorated, serviceProvider);

            return (T)proxy;
        }

        private void SetParameters(T decorated, IServiceProvider serviceProvider)
        {
            _decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
            _serviceProvider = serviceProvider;
        }

        private IInterceptor[] GetInterceptors(MethodInfo targetMethod)
        {
            string key = $"{_decorated.GetType().Name}{targetMethod.Name}";
            if (!_interceptors.TryGetValue(key, out var interceptors))
            {
                interceptors = ProxifyContext
                    .InterceptorTypes
                    .Select(type => _serviceProvider.GetService(type) as IInterceptor)
                    .ToArray();

                _interceptors.TryAdd(key, interceptors);
            }

            return interceptors ?? new IInterceptor[0];
        }
    }
}
