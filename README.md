# Bauen

```bash
dotnet build
```

#  Packen

```bash
dotnet pack src/PixieValidation/PixieValidation.csproj -c Release
```

# Package pushen

```bash
VERSION=$(grep -oP '(?<=<Version>)[^<]+' Directory.Build.props)
dotnet nuget push src/PixieValidation/bin/Release/PixieValidation.$VERSION.nupkg \
  --source "https://nuget.pkg.github.com/sallycloud/index.json" \
  --api-key $GITHUB_PAT
```