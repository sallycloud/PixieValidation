# Build

```bash
dotnet build
```

#  Pack

```bash
dotnet pack src/PixieValidation/PixieValidation.csproj -c Release
```

# Push to package registry

```bash
VERSION=$(grep -oP '(?<=<Version>)[^<]+' Directory.Build.props)
dotnet nuget push src/PixieValidation/bin/Release/PixieValidation.$VERSION.nupkg \
  --source github \
  --api-key $GITHUB_PAT
```

# Valid<T>

`Valid<T>` is proof that a value has been successfully validated. This guarantee is
intentionally narrow: a `Valid<T>` can only be created via `T`'s own `IValidatable`
implementation (through `ToValidOrThrow()` in `ValidatableExtensions`).

An external `IValidator<T>` or a `PropChecker<T>` can validate a value (`Validate()`,
`ValidateOrThrow()`), but can never produce a `Valid<T>`. If it could, any caller could obtain
a `Valid<T>` by supplying a lax or empty `IValidator<T>` — the guarantee would then say
nothing meaningful about which rules were actually applied.