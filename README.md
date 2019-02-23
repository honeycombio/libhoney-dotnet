# Honeycomb dotnet implementation

## ASP.NET Core

### Services Registration
```csharp

  public void ConfigureServices(IServiceCollection services)
  {
  ...
      services.AddHoneycomb(new HoneycombApiSettings {
                TeamId = "blah",
                BatchSize = 20,
                SendFrequency = 60000,
                DefaultDataSet = "MyTestDataSet"
            });
  ...
  }
```

### Middleware

Note the relative position to app.UseMvc()

```csharp
  public void Configure(IApplicationBuilder app, IHostingEnvironment env)
  {
      ...
      app.UseHoneycomb();

      app.UseMvc();

  }

```


### Configuration

Configuration can either be done through adding this to your appSettings.json

```json
{
  "HoneycombSettings": {
    "TeamId": "",
    "DefaultDataSet": "",
    "BatchSize": 100,
    "SendFrequency": 10000
  }
}
```

Or alternatively, you can create an instance of  `HoneycombApiSettings` and pass it directly to the Service registration:

```csharp
services.AddHoneycomb(new HoneycombApiSettings {
                TeamId = "blah",
                BatchSize = 20,
                SendFrequency = 60000,
                DefaultDataSet = "MyTestDataSet"
            });
```

