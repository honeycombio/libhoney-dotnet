# libhoney-dotnet [![CircleCI](https://circleci.com/gh/honeycombio/libhoney-dotnet.svg?style=shield)](https://circleci.com/gh/honeycombio/libhoney-dotnet)

This package makes it easy to instrument your .NET app to send useful events to Honeycomb, a service for debugging your software in production.

- [Sample](/Sample/)

## Requirements

The library targets [.NET Standard 2.0](https://dotnet.microsoft.com/platform/dotnet-standard).

To build and test the project locally, you'll need .NET SDK 5.0+.

## Building & Testing

To build all the projects, run the following:

`dotnet build`

To run tests, run:

`dotnet test`

To create the nuget packages, run:

`dotnet pack`

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
        app.UseHoneycomb();

        app.UseMvc();
        ...
    }
```

### Configuration

Configuration can either be done through adding this to your appSettings.json

```json
{
  "HoneycombSettings": {
    "WriteKey": "<your-writekey>",
    "DefaultDataSet": "<your-dataset>",
    "BatchSize": 100,
    "SendFrequency": 10000
  }
}
```

Or alternatively, you can configure an instance of `HoneycombApiSettings` programmatically:

```csharp
    using Honeycomb.Models;
    ...

    services.AddHoneycomb(options => {
        options.ApiHost = "https://api.honeycomb.io";
        options.WriteKey = "<your-writekey>";
        options.DefaultDataSet = "<your-dataset>";
        options.BatchSize = 100;
        options.SendFrequency = 10000;
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

## Donation

This project was kindly donated to Honeycomb by [@martinjt](https://github.com/martinjt).
