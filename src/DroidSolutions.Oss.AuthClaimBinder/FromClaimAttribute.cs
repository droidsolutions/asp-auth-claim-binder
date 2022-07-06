using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DroidSolutions.Oss.AuthClaimBinder;

/// <summary>
/// A custom attribute to be used in API controllers when they want to retrieve values from a claim.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class FromClaimAttribute : Attribute, IBindingSourceMetadata
{
  /// <inheritdoc/>
  public BindingSource BindingSource => ClaimBindingSource.Claim;
}
