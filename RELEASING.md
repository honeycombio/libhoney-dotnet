# Releasing Process

1. Add changelog entry
2. Update Version in both Honeycomb and Honeycomb.AspNet csproj files
3. Open a PR with the above, and merge that into main
4. Tag the merged commit with the new version (e.g. `v1.2.0`)
5. Push the tag upstream (this will kick off the release pipeline in CI)
6. Once the CI is done, publish the GitHub draft release as pre-release through GitHub UI
7. Update public docs with the new version
