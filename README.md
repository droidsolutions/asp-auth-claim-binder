# DroidSolutions Auth Claim Binder

Custom modelbinder for ASP.NET Core MVC (and Web APIs) to allow injecting claims into controller actions.

[![Coverage Status](https://coveralls.io/repos/github/droidsolutions/asp-auth-claim-binder/badge.svg?branch=main)](https://coveralls.io/github/droidsolutions/asp-auth-claim-binder?branch=main)
![Nuget](https://img.shields.io/nuget/v/DroidSolutions.Oss.AuthClaimBinder)
[![semantic-release](https://img.shields.io/badge/%20%20%F0%9F%93%A6%F0%9F%9A%80-semantic--release-e10079.svg)](https://github.com/semantic-release/semantic-release)

This NuGet package contains the `FromClaim` attribute that can be used in controller actions to inject a value from a claim, for example the user id or role. It also offers a ASP.NET Core Modelbinder and a Modelbinder provider.

This project was inspired by [this blogpost](https://www.davidkaya.com/custom-from-attribute-for-controller-actions-in-asp-net-core/).

# Installation

You can grab this NuGet package from [NuGet.org](https://www.nuget.org/packages/DroidSolutions.Oss.AuthClaimBinder).

# How it works

The modelbinder will search available claims from the authentication for the given name you used as argument name in your controller action. Specifically the claims on the user property in the `HttpContext` objects are used.
If a claim with the given name is found the modelbinder will try to convert the value to the type you have specified. Currently the following types are supported:

- `string`
- `Guid`
- `Enum`

**Note:** Which claims exist in the User object is dependent on your authentication middleware and out of the scope of this repository. For example you can extend the `AuthenticationHandler` like described in [the official docs](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-6.0#authentication-handler) and add custom claims to the user.

# Usage

To use the attribute first the `ClaimModelBinderProvider` must be added to the list of `ModelBinderProviders`.

## Register

The `ClaimModelBinderProvider` can be added to the MVC options (when using Web API projects) like this

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

This is a dictionary of string keys (the key you want to use as argument names) and a list of strings that serve as aliases. For example if you use Open ID Connect and get you claims from the JWT they might be some long strings or urls. The example below uses the key `role` and adds an alias for `System.Security.Claims.ClaimTypes.Role`. This way the binder finds the value of the claim with the name of the `ClaimTypes.Role` when you use `role` as the argument name.

```cs
builder.Services.Configure<ClaimBinderSettings>(o => o.AliasConfig = new Dictionary<string, List<string>>
{
  { "role", new List<string> { ClaimTypes.Role } },
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

## Exceptions

There are special exceptions for errors during parsing of claim values which are explained below:

### MissingClaimException

When the `FromClaim` attribute is used but the claim (or it's alias) can not be found in the user claims, this exception is thrown. This is especially useful, if you want to show the caller of your API a BadRequest response or an message.

For example, let's assume you want to use a value from a special header you defined. You have set up your authentication handler to get the value from the header and put it in the user claims:
```cs
// Authorization handler
if (Request.Headers.TryGetValue("x-myvalue", out StringValues namespaceHeader))
{
  claims = claims.Append(new Claim("myvalue", namespaceHeader[0]));
}

// Contoller
public async IActionResult MyMethod([FromClaim] string myvalue)
{
  // do something with myvalue
}
```

This works, when the x-myvalue header is provided, but if it is not, than the exception would be thrown (probably leading to a 500 beeing returned). Since you know the exception that is thrown you can set up an [Exception Filter](https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters) or a special controller action that handles errors and process the `MissingClaimException`. See [the ASP.NET Core docs](https://learn.microsoft.com/en-us/aspnet/core/web-api/handle-errors) for more info on how to set up error handling.

Thie `MissingClaimException` contains a property with the name of the claim. Be aware, that this is the name used in the controller attribute, so in case of the header example you probably need to write a custom message, indicating that the header is missing.

### ClaimParsingException

This exception is thrown when a value cannot be parsed to the specified type. For example let's assume you have a Guid user id and want to use it in your controller:
```cs
// Contoller
public async IActionResult MyMethod([FromClaim] Guid user)
{
  // do something with user Id
}
```

Dependent on how you get the user claim it could be possible that it is not a valid Guid. In this case the `ClaimModelBinder` would throw a `ClaimParsingException` with the name of the claim ("user" in this case) and the destination type (`Guid`). This can help you set up special error handling for those cases.

# Development

If you want to add a feature or fix a bug, be sure to read the [contribution guidelines](./CONTRIBUTING.md) first before open a pull request.

You'll need to install the .NET SDK which can be downloaded [here](https://dotnet.microsoft.com/en-us/download).

To build the project, just run `dotnet build` in the repository root. Tests can be executed with `dotnet test` and code coverage is generated by either running `dotnet test --collect:"XPlat Code Coverage"` or `dotnet test /p:CollectCoverage=true`.