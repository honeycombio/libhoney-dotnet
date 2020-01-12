## Integrating with AspNet Core

### Before you get started
You'll need your Honeycomb TeamId, this is your API Key please find it below

```
${TeamId}
```

### Add the package

The Honeycomb AspNet core integration is setup in the package `Honeycomb.AspNetCore` On nuget.

```cmd
dotnet add package Honeycomb.AspNetCore
```

You can also add the above package through visual studio.

Then, in your projects `appsettings.json` add the following settings:

```json
{
  "HoneycombSettings": {
    "TeamId": "blah",
    "DefaultDataSet": "MyTestDataSet",
    "BatchSize": 100,
    "SendFrequency": 10000
  }
}
```

### Configure the pipelines

In your `Startup.cs` file, add the following at the top:

```csharp
using Honeycomb.AspNetCore.Middleware;
```

Then find the `ConfigureServices()` method and add the `AddHoneycomb(Configuration)` method like below.

```csharp
    public void ConfigureServices(IServiceCollection services)
    {
    ...
        services.AddHoneycomb(Configuration);
    ...
    }
```

Then find the `Configure()` method in the same file and add `app.UseHoneycomb()` BEFORE `app.UseMvc()`

```csharp
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        ...
        app.UseHoneycomb();

        app.UseMvc();
        ...
    }
```

### Default Event Data

You will now be getting some default event data submitted to Honeycomb, use the below to get more information.

### Add Some event data

In any class that it created through DI, you can pull in the `IHoneycombEventManager`, and this will allow you to add events to the current request context as below:

```csharp

    public class HomeController : Controller
    {
        private readonly IHoneycombEventManager _eventManager;

        public HomeController(IHoneycombEventManager eventManager)
        {
            _eventManager = eventManager;
        }

        ...

        public IActionResult MyAction()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            ...
            result = GetDataFromAPI();
            ...
            stopWatch.Stop();
            _eventManager.AddData("api_response_ms", stopWatch.ElapsedMilliseconds);

            return result;
        }
    }
```
