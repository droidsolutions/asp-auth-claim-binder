namespace DroidSolutions.Oss.AuthClaimBinder.Settings;

/// <summary>
/// Settings for the claim model binder.
/// </summary>
public class ClaimBinderSettings
{
  /// <summary>
  /// Gets or sets a list of aliases to use when searching for claims.
  /// </summary>
  public Dictionary<string, List<string>>? AliasConfig { get; set; }
}
