# DroidSolutions Auth Claim Binder

Custom modelbinder for ASP.NET Core MVC to allow injecting claims into controller actions.

[![Coverage Status](https://coveralls.io/repos/github/droidsolutions/semantic-version/badge.svg?branch=main)](https://coveralls.io/github/droidsolutions/semantic-version?branch=main)
![Nuget](https://img.shields.io/nuget/v/DroidSolutions.Oss.AuthClaimBinder)
[![semantic-release](https://img.shields.io/badge/%20%20%F0%9F%93%A6%F0%9F%9A%80-semantic--release-e10079.svg)](https://github.com/semantic-release/semantic-release)

This NuGet package contains the `FromClaim` attribute that can be used in controller actions to inject a value from a claim, for example the user id or role. It also offers a ASP.NET Core Modelbinder and a Modelbinder provider.

This project wass inspired by [this blogpost](https://www.davidkaya.com/custom-from-attribute-for-controller-actions-in-asp-net-core/).

# Installation

You can grab this NuGet package from [NuGet.org](https://www.nuget.org).

# How it works

The modelbinder will search available claims from the authentication for the given name you used as argument name in your controller action. Specifically the claims on the user property in the HttpContext objects are used.
If a claim with the given name is found the modelbinder will try to convert the value to the type you have specified. Currently the following types are supported:

- `string`
- `Guid`
- `Enum`

# Usage

To use the attribute first the modelbinder provider must be added to the list of `ModelBinderProviders`.

## Register

The modelbinder provider can be added to the MVC options like this

```cs
builder.Services.AddControllers(options =>
  {
    options.ModelBinderProviders.Insert(0, new ClaimModelBinderProvider());
  });
```

or you when using MVC you can use

```cs
builder.Services.AddMvc(options =>
  {
    options.ModelBinderProviders.Insert(0, new ClaimModelBinderProvider());
  });
```

See the [official documentation](https://docs.microsoft.com/en-us/aspnet/core/mvc/advanced/custom-model-binding?view=aspnetcore-6.0#implementing-a-modelbinderprovider) for more info.

## Configuration

The `ClaimsModelBinder` can be configured via `ClaimBinderSettings`. Those settings are retrieved via `IOptions<ClaimBinderSettings>` so you just need to configure it setting up your dependency injection.

### AliasConfig

If the claims you have from your authentication method are complex or you want to use other argument names in your controller actions you can provide an alias list via `ClaimBinderSettings.AliasConfig`.

This is a dictionary of string keys (the key you want to use as argument names) and a list of strings that serve as aliases. For example if you use Open ID Connect and get you claims from the JWT they might be some long strings or urls. The example below uses the key `user` and adds an alias for `System.Security.Claims.ClaimTypes.NameIdentifier`. This way the binder finds the value of the claim with the name of the `ClaimTypes.NameIdentifier` when you use `user` as the argument name.

```cs
builder.Services.Configure<ClaimBinderSettings>(new ClaimBinderSettings
{
  AliasConfig = new Dictionary<string, List<string>>
  {
    { "user", new List<string> { ClaimTypes.NameIdentifier } },
    { "role", new List<string> { ClaimTypes.Role } },
  },
});
```

## Use the attribute

To use it simply add the `FromClaim` attribute before your method parameter. The name of the argument is the name of the claim that is searched (or one of the aliases you have configured) and the value will be converted to the type you used. See above for a list of supported types.

```cs
public async Task<IActionResult> DoSomething([FromClaim] string user, [FromClaim] BasicAuthRole role, CancellationToken cancellationToken)
{
  // ...
}
```

# Development

If you want to add a feature or fix a bug, be sure to read the [contribution guidelines](./CONTRIBUTING.md) first before open a pull request.

You'll need to install the .NET SDK which can be downloaded [here](https://dotnet.microsoft.com/en-us/download).

To build the project, just run `dotnet build` in the repository root. Tests can be executed with `dotnet test` and code coverage is generated by either running `dotnet test --collect:"XPlat Code Coverage"` or `dotnet test /p:CollectCoverage=true`.