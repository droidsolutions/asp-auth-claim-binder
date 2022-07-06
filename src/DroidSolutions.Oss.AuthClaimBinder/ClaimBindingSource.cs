using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DroidSolutions.Oss.AuthClaimBinder;

/// <summary>
/// A binding source that provides a value from the user claim.
/// </summary>
public class ClaimBindingSource : BindingSource
{
  /// <summary>
  /// Gets the binding source.
  /// </summary>
  /// <returns>The binding source.</returns>
  public static readonly BindingSource Claim = new ClaimBindingSource("Claim", "Claim", true, true);

  /// <summary>
  /// Initializes a new instance of the <see cref="ClaimBindingSource"/> class.
  /// </summary>
  /// <param name="id">The id, a unique identifier.</param>
  /// <param name="displayName">The display name.</param>
  /// <param name="isGreedy">A value indicating whether or not the source is greedy.</param>
  /// <param name="isFromRequest">A value indicating whether or not the data comes from the HTTP request.</param>
  public ClaimBindingSource(string id, string displayName, bool isGreedy, bool isFromRequest)
    : base(id, displayName, isGreedy, isFromRequest)
  {
  }

  /// <inheritdoc/>
  public override bool CanAcceptDataFrom(BindingSource bindingSource)
  {
    return bindingSource == Custom || bindingSource == this;
  }
}
