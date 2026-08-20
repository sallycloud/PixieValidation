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
dotnet nuget push src/PixieValidation/bin/Release/PixieValidation.0.1.0.nupkg \
  --source "https://nuget.pkg.github.com/sallycloud/index.json" \
  --api-key $GITHUB_PAT
```