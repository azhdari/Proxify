using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mohmd.AspNetCore.Proxify.Internal;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mohmd.AspNetCore.Proxify
{
    public class DispatchProxy<T> : DispatchProxy
        where T : class
    {
        private T _decorated;
        private ILogger _logger;
        private IServiceProvider _serviceProvider;
        private ConcurrentDictionary<string, IInterceptor[]> _layers = new ConcurrentDictionary<string, IInterceptor[]>();

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            if (targetMethod != null)
            {
                try
                {
                    try
                    {
                        Run(InterceptorStep.Before, targetMethod, args, null);
                    }
                    catch (Exception ex)
                    {
                        //Do not stop method execution if exception  
                        LogException(ex);
                    }

                    var result = targetMethod.Invoke(_decorated, args);
                    if (result is Task resultTask)
                    {
                        resultTask.ContinueWith(task =>
                            {
                                if (task.Exception != null)
                                {
                                    Run(InterceptorStep.OnException, targetMethod, args, task.Exception.InnerException ?? task.Exception);
                                }
                                else
                                {
                                    object taskResult = null;
                                    if (task.GetType().GetTypeInfo().IsGenericType)
                                    {
                                        var property = task.GetType().GetTypeInfo().GetProperties()
                                            .FirstOrDefault(p => p.Name == "Result");
                                        if (property != null)
                                        {
                                            taskResult = property.GetValue(task);
                                        }
                                    }

                                    Run(InterceptorStep.After, targetMethod, args, null, taskResult);
                                }
                            });
                    }
                    else
                    {
                        try
                        {
                            Run(InterceptorStep.After, targetMethod, args, null, result);
                        }
                        catch (Exception ex)
                        {
                            //Do not stop method execution if exception  
                            LogException(ex);
                        }
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    if (ex is TargetInvocationException)
                    {
                        Run(InterceptorStep.OnException, targetMethod, args, ex.InnerException ?? ex);
                        throw ex.InnerException ?? ex;
                    }
                }
            }

            throw new ArgumentException(nameof(targetMethod));
        }

        public static T Create(T decorated, ILogger logger, IServiceProvider serviceProvider)
        {
            //object proxy = CreateInstance(decorated.GetType());
            object proxy = Create<T, DispatchProxy<T>>();
            ((DispatchProxy<T>)proxy).SetParameters(decorated, logger, serviceProvider);

            return (T)proxy;
        }

        private void SetParameters(T decorated, ILogger logger, IServiceProvider serviceProvider)
        {
            if (decorated == null)
            {
                throw new ArgumentNullException(nameof(decorated));
            }

            _decorated = decorated;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        private void Run(InterceptorStep position, MethodInfo targetMethod, object[] args, Exception exception = null, object result = null)
        {
            var interceptors = GetLayers(targetMethod);

            if (position == InterceptorStep.Before)
            {
                interceptors = interceptors.OrderBy(x => x.Priority).ToArray();
            }
            else
            {
                interceptors = interceptors.OrderByDescending(x => x.Priority).ToArray();
            }

            IInvocation invocation = new Invocation(_decorated, targetMethod, args, exception, result);

            switch (position)
            {
                case InterceptorStep.Before:
                    foreach (var item in interceptors)
                    {
                        try
                        {
                            item.InvokeBefore(invocation);
                        }
                        catch (NotImplementedException)
                        {
                        }
                    }
                    break;
                case InterceptorStep.After:
                    foreach (var item in interceptors)
                    {
                        try
                        {
                            item.InvokeAfter(invocation);
                        }
                        catch (NotImplementedException)
                        {
                        }
                    }
                    break;
                case InterceptorStep.OnException:
                    foreach (var item in interceptors)
                    {
                        try
                        {
                            item.InvokeOnException(invocation);
                        }
                        catch (NotImplementedException)
                        {
                        }
                    }
                    break;
            }
        }

        private IInterceptor[] GetLayers(MethodInfo targetMethod)
        {
            string key = $"{_decorated.GetType().Name}{targetMethod.Name}";
            if (!_layers.TryGetValue(key, out var layers))
            {
                layers = ProxifyContext
                    .LayerTypes
                    .Select(type => ActivatorUtilities.CreateInstance(_serviceProvider, type) as IInterceptor)
                    .ToArray();

                _layers.TryAdd(key, layers);
            }

            return layers ?? new IInterceptor[0];
        }

        private void LogException(Exception exception, MethodInfo methodInfo = null)
        {
            try
            {
                var errorMessage = new StringBuilder();
                errorMessage.AppendLine($"Class {_decorated.GetType().FullName}");
                errorMessage.AppendLine($"Method {methodInfo?.Name} threw exception");

                _logger?.LogError(exception, errorMessage.ToString());
            }
            catch (Exception)
            {
                // ignored  
                //Method should return original exception  
            }
        }
    }
}
