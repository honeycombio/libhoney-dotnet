# libhonet-dotnet Changelog

## v.1.3.0

### Improvements

- Add Environment and Services support (#60) | [@MikeGoldsmith](https://github.com/MikeGoldsmith)

### Maintenance

- Add re-triage workflow (#50)| [@vreynolds](https://github.com/vreynolds)

### Dependencies

- Bump Microsoft.Extensions.Hosting from 6.0.0 to 6.0.1 (#62)
- Bump Moq from 4.16.1 to 4.17.2 (#61)
- Bump Microsoft.AspNetCore.Routing from 2.1 to 2.2.2 (#55)
- Bump Microsoft.NET.Test.Sdk from 15.9.0 to 17.1.0 (#58)
- Bump Newtonsoft.Json from 9.0.1 to 13.0.1 (#57)
- Bump Microsoft.Extensions.Http from 2.1 to 6.0.0 (#51)
- Bump Microsoft.AspNetCore.Http from 2.1 to 2.2.2 (#53)
- Bump Moq from 4.10.1 to 4.16.1 (#54)
- Bump Microsoft.Extensions.Logging from 2.1 to 6.0.0 (#46)
- Bump Microsoft.Extensions.Hosting from 2.1 to 6.0.0 (#47)

## v1.2.0

### Inprovements

- Fix authors, move examples and target netstandard2.0 (#48)

### Maintenance

- Add dependabot (#42)
- Empower apply-labels action to apply labels (#41)
- Adds Stalebot (#40)
- Change maintenance badge to maintained (#39)
- Add issue and PR templates (#38)
- Add community health files (#36)
- Add OSS lifecycle badge (#37)
- Updates Github Action Workflows (#35)
- Switches CODEOWNERS to telemetry-team (#33)

### Dependencies

- Bump Shouldly from 3.0.2 to 4.0.3 (#44)
- Bump xunit.runner.visualstudio from 2.4.0 to 2.4.3 (#45)
- Bump xunit from 2.4.0 to 2.4.1 (#43)

## v1.1.1

### Improvements

- Add WriteKey and deprecate TeamId (#30)

### Fixes

- Update AddHoneycomb with settings instance to use Action<> (#29)
- Fix typo in VersionSuffux project file property (#31)
- Update default SendFrequency to 10 seconds (#27)
- Stopping the timer before calculating the elapsedmilliseconds (#28)

## v1.1.0

### Improvements

- Change the ApiHost setting to include the url scheme (#21)
- Add support for setting the api send target through configuration (#20)
