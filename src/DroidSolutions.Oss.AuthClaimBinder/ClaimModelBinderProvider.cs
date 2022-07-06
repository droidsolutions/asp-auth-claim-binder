using DroidSolutions.Oss.AuthClaimBinder.Settings;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DroidSolutions.Oss.AuthClaimBinder;

/// <summary>
/// Provides the <see cref="ClaimBindingSource"/>.
/// </summary>
public class ClaimModelBinderProvider : IModelBinderProvider
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ClaimModelBinderProvider"/> class.
  /// </summary>
  public ClaimModelBinderProvider()
  {
  }

  /// <inheritdoc/>
  public IModelBinder? GetBinder(ModelBinderProviderContext context)
  {
    if (context.BindingInfo.BindingSource?.CanAcceptDataFrom(ClaimBindingSource.Claim) == true)
    {
      return new ClaimModelBinder(
        context.Services.GetRequiredService<ILogger<ClaimModelBinder>>(),
        context.Services.GetService<IOptions<ClaimBinderSettings>>()?.Value);
    }
    else
    {
      return null;
    }
  }
}
