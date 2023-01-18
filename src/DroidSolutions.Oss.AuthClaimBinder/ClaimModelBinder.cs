using System.Security.Claims;

using DroidSolutions.Oss.AuthClaimBinder.Exceptions;
using DroidSolutions.Oss.AuthClaimBinder.Settings;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace DroidSolutions.Oss.AuthClaimBinder;

/// <summary>
/// Extracts a value from the user claims previously set by authentication middleware.
/// </summary>
public class ClaimModelBinder : IModelBinder
{
  private readonly ILogger _logger;
  private readonly ClaimBinderSettings? _settings;

  /// <summary>
  /// Initializes a new instance of the <see cref="ClaimModelBinder"/> class.
  /// </summary>
  /// <param name="logger">An instance of a logger.</param>
  /// <param name="settings">The configuration for the claim model binder.</param>
  public ClaimModelBinder(ILogger<ClaimModelBinder> logger, ClaimBinderSettings? settings)
  {
    _logger = logger;
    _settings = settings;
  }

  /// <inheritdoc/>
  public Task BindModelAsync(ModelBindingContext bindingContext)
  {
    Claim? claim = GetClaim(bindingContext.HttpContext.User.Claims, bindingContext.FieldName);

    if (claim == null)
    {
      bindingContext.Result = ModelBindingResult.Failed();

      _logger.LogError("The claim {FieldName} could not be extracted from the user.", bindingContext.FieldName);

      throw new MissingClaimException(bindingContext.FieldName);
    }

    if (bindingContext.ModelType == typeof(Guid))
    {
      try
      {
        bindingContext.Result = ModelBindingResult.Success(Guid.Parse(claim.Value));
      }
      catch (Exception ex)
      {
        bindingContext.Result = ModelBindingResult.Failed();
        _logger.LogError(ex, "The claim {FieldName} could not be parsed to a Guid!", bindingContext.FieldName);

        throw new ClaimParsingException(
          $"The claim {bindingContext.FieldName} could not be parsed to a Guid!",
          ex,
          bindingContext.FieldName,
          bindingContext.ModelType);
      }
    }
    else if (bindingContext.ModelType.IsEnum)
    {
      if (!Enum.TryParse(bindingContext.ModelType, claim.Value, false, out var value))
      {
        throw new ClaimParsingException(
          $"The value {claim.Value} of the claim {bindingContext.FieldName} could not be parsed to the enum {bindingContext.ModelType.Name}.",
          null,
          bindingContext.FieldName,
          bindingContext.ModelType);
      }

      bindingContext.Result = ModelBindingResult.Success(value);
    }
    else
    {
      bindingContext.Result = ModelBindingResult.Success(claim.Value);
    }

    return Task.CompletedTask;
  }

  private Claim? GetClaim(IEnumerable<Claim> claims, string name)
  {
    Claim? claim = claims.FirstOrDefault(c => c.Type == name);

    if (claim == null && _settings?.AliasConfig?.TryGetValue(name, out List<string>? aliases) == true)
    {
      foreach (var alias in aliases)
      {
        claim = claims.FirstOrDefault(c => c.Type == alias);
        if (claim != null)
        {
          break;
        }
      }
    }

    return claim;
  }
}
