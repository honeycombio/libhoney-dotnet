## Integrating with AspNet Core

### Before you get started
You'll need your Honeycomb TeamId, this is your API Key please find it below

```
${TeamId}
```

### Add the package

The Honeycomb AspNet core integration is setup in the package `Honeycomb.AspNetCore` On nuget.

```cmd
dotnet package add Honeycomb.AspNetCore
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