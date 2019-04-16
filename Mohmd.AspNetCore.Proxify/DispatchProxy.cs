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
    {
        private T _decorated;
        private ILogger _logger;
        private IServiceProvider _serviceProvider;
        private ConcurrentDictionary<string, ILayer[]> _layers = new ConcurrentDictionary<string, ILayer[]>();

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            if (targetMethod != null)
            {
                try
                {
                    try
                    {
                        Run(LayerPosition.Before, targetMethod, args, null);
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
                                    Run(LayerPosition.OnException, targetMethod, args, task.Exception.InnerException ?? task.Exception);
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

                                    Run(LayerPosition.After, targetMethod, args, null, taskResult);
                                }
                            });
                    }
                    else
                    {
                        try
                        {
                            Run(LayerPosition.After, targetMethod, args, null, result);
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
                        Run(LayerPosition.OnException, targetMethod, args, ex.InnerException ?? ex);
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

        private void Run(LayerPosition position, MethodInfo targetMethod, object[] args, Exception exception = null, object result = null)
        {
            var layers = GetLayers(targetMethod);

            switch (position)
            {
                case LayerPosition.Before:
                    foreach (var item in layers)
                    {
                        try
                        {
                            item.InvokeBefore(targetMethod, args);
                        }
                        catch (NotImplementedException)
                        {
                        }
                    }
                    break;
                case LayerPosition.After:
                    foreach (var item in layers)
                    {
                        try
                        {
                            item.InvokeAfter(targetMethod, args, result);
                        }
                        catch (NotImplementedException)
                        {
                        }
                    }
                    break;
                case LayerPosition.OnException:
                    foreach (var item in layers)
                    {
                        try
                        {
                            item.InvokeOnException(targetMethod, exception);
                        }
                        catch (NotImplementedException)
                        {
                        }
                    }
                    break;
            }
        }

        private ILayer[] GetLayers(MethodInfo targetMethod)
        {
            string key = $"{_decorated.GetType().Name}{targetMethod.Name}";
            if (!_layers.TryGetValue(key, out var layers))
            {
                layers = ProxifyContext
                    .LayerTypes
                    .Select(type => ActivatorUtilities.CreateInstance(_serviceProvider, type) as ILayer)
                    .ToArray();

                _layers.TryAdd(key, layers);
            }

            return layers ?? new ILayer[0];
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
