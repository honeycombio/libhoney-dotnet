# Honeycomb dotnet implementation

## ASP.NET Core

### Services Registration
```csharp
    using Honeycomb.AspNetCore.Middleware;
    ...

    public void ConfigureServices(IServiceCollection services)
    {
    ...
        services.AddHoneycomb(Configuration);
    ...
    }
```

### Middleware

Note the relative position to app.UseMvc()

```csharp
    using Honeycomb.AspNetCore.Middleware;
    ...

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        ...
        app.UseHoneycomb(Configuration);

        app.UseMvc();
        ...
    }

```

### Configuration

Configuration can either be done through adding this to your appSettings.json

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

Or alternatively, you can create an instance of  `HoneycombApiSettings` and pass it directly to the Service registration:

```csharp
    using Honeycomb.Models;
    ...

    services.AddHoneycomb(new HoneycombApiSettings {
        TeamId = "blah",
        DefaultDataSet = "MyTestDataSet"
        BatchSize = 100,
        SendFrequency = 10000,
    });
```

## Usage

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