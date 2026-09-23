# Getting Started

Define your type and how to validate it by implementing `IValidatable`:

\```csharp
using PixieValidation;
using PixieValidation.PropCheckers;

public partial record User : IValidatable
{
public required string Name { get; init; }
public required string Email { get; init; }

    private static readonly PropChecker<string> NameChecker =
        StringCheckers.NotEmpty();

    private static readonly PropChecker<string> EmailChecker =
        EmailCheckers.IsValid();

    public void ValidateAndCollect(ErrorCollector errors)
    {
        errors.Check(Name, NameChecker);
        errors.Check(Email, EmailChecker);
    }
}
\```

Then validate an instance:

\```csharp
var user = new User { Name = "", Email = "not-an-email" };

// Throws a ValidationException listing every error, with paths like "Name" and "Email"
user.ValidateOrThrow();

// Or collect errors without throwing
var errors = user.Validate();
foreach (var error in errors)
Console.WriteLine($"{error.Path}: {error.Message}");
\```

Nested objects and collections (lists, sets, dictionaries) are validated automatically via `errors.Nested(...)` — see the `PropCheckers` namespace for the full set of built-in rule functions (strings, numbers, dates, GUIDs, enums, collections, and more).

If you need proof, carried in the type system, that a value has actually been validated, use `ToValidOrThrow()` instead of `ValidateOrThrow()` — it returns a `Valid<User>` rather than a plain `User`. See [`Valid<T>`](#validt) below for why that guarantee is narrower than it might look.

# Valid<T>

`Valid<T>` is proof that a value has been successfully validated. This guarantee is
intentionally narrow: a `Valid<T>` can only be created via `T`'s own `IValidatable`
implementation (through `ToValidOrThrow()` in `ValidatableExtensions`).

An external `IValidator<T>` or a `PropChecker<T>` can validate a value (`Validate()`,
`ValidateOrThrow()`), but can never produce a `Valid<T>`. If it could, any caller could obtain
a `Valid<T>` by supplying a lax or empty `IValidator<T>` — the guarantee would then say
nothing meaningful about which rules were actually applied.

# Build

\```bash
dotnet build
\```

#  Pack

\```bash
dotnet pack src/PixieValidation/PixieValidation.csproj -c Release
\```

# Releasing

Releases are published automatically to nuget.org via GitHub Actions (Trusted Publishing,
no API key involved). To publish a new version:

1. Bump `<Version>` in `Directory.Build.props`.
2. Commit and push the change.
3. Tag the commit with a `v`-prefixed version and push the tag:

\```bash
VERSION=$(grep -oP '(?<=<Version>)[^<]+' Directory.Build.props)
git tag "v$VERSION"
git push origin "v$VERSION"
\```

Pushing the tag triggers `.github/workflows/publish.yml`, which builds, tests, packs, and
publishes the package to nuget.org.