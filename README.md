# Proxify
Simple, Fast and reliable Dynamic Proxy for ASP.NET Core 2.0 and later

[![License](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](https://github.com/azhdari/Mohmd.JsonResources/blob/master/License.txt)
[![NuGet](https://img.shields.io/badge/nuget-1.1.2-blue.svg?style=flat-square)](https://www.nuget.org/packages/Mohmd.AspNetCore.Proxify/1.1.2)

## Getting Started
Use these instructions to get the package and use it.

### Install
From the command prompt
```bash
dotnet add package Mohmd.AspNetCore.Proxify
```
or
```bash
Install-Package Mohmd.AspNetCore.Proxify
```
or
```bash
paket add Mohmd.AspNetCore.Proxify
```

### Configure
Add service
```csharp
public void ConfigureServices(IServiceCollection services)
{
  services.AddProxify()
      // add every assembly that might contains interceptors
      .AddAssemblyByType<Startup>()
      .Build()
      // then add services to IoC
      .AddScopedProxyService<ISampleService, SampleService>();

  // you can add services separately.
  services.AddTransientProxyService<ISample2Service, Sample2Service>();
}
```

Create interceptors this way:

```csharp
public class ProfilingInsterceptor : IInterceptor
{
    private Stopwatch _stopwatch = new Stopwatch();
    private readonly ILogger _logger;

    public ProfilingInsterceptor(ILogger<ProfilingInsterceptor> logger)
    {
        _logger = logger;
    }

    // we can prioritize interceptors
    public int Priority => 2;

    public async Task Intercept(IInvocation invocation)
    {
        try
        {
            // add codes to run before invoking the target method
            _stopwatch.Start();

            // run next interceptor or invoke the target method
            await invocation.Proceed();

            // if we reach here, it means invoking the target method
            // is finished, now if the target method had a return value
            // we can access it
            var returnValue = invocation.ReturnValue;
        }
        catch (Exception ex)
        {
            // add codes to run on exceptions
        }
        finally
        {
            // add codes to run after invoking the target method
            _stopwatch.Stop();
            _logger.LogInformation($"Invoking {invocation.Method.Name} took {_stopwatch.Elapsed.ToString()} = `{invocation.ReturnValue}`.");
        }
    }
}
```












