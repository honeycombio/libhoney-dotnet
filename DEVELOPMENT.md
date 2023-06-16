# Local Development

## Requirements

The library targets [.NET Standard 2.0](https://dotnet.microsoft.com/platform/dotnet-standard).

To build and test the project locally, you'll need .NET SDK 5.0+.

## Building & Testing

To build all the projects, run the following:

```shell
dotnet build
```

To run all tests, run:

```shell
dotnet test
```

To run individual tests:

```shell
dotnet test --filter QueueTests
```

To create the nuget packages, run:

```shell
dotnet pack
```
