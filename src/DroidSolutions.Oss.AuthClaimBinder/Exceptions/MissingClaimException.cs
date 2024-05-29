using System.Runtime.Serialization;

namespace DroidSolutions.Oss.AuthClaimBinder.Exceptions;

/// <summary>
/// Special exception when a claim that is bound via model binder can not be found.
/// </summary>
[Serializable]
public class MissingClaimException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="MissingClaimException"/> class.
  /// </summary>
  public MissingClaimException()
  {
    ClaimName = string.Empty;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="MissingClaimException"/> class.
  /// </summary>
  /// <param name="name">The name of the missing claim.</param>
  public MissingClaimException(string name)
    : this($"The claim \"{name}\" was not found.", name)
  { }

  /// <summary>
  /// Initializes a new instance of the <see cref="MissingClaimException"/> class.
  /// </summary>
  /// <param name="message">The exception message.</param>
  /// <param name="name">The name of the missing claim.</param>
  public MissingClaimException(string message, string name)
    : base(message)
  {
    ClaimName = name;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="MissingClaimException"/> class.
  /// </summary>
  /// <param name="message">The exception message.</param>
  /// <param name="inner">The inner exception, if any.</param>
  public MissingClaimException(string? message, Exception? inner)
    : base(message, inner)
  {
    ClaimName = string.Empty;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="MissingClaimException"/> class.
  /// </summary>
  /// <param name="message">The exception message.</param>
  /// <param name="inner">The inner exception, if any.</param>
  /// <param name="name">The name of the missing claim.</param>
  public MissingClaimException(string? message, Exception? inner, string name)
    : base(message, inner)
  {
    ClaimName = name;
  }

  /// <summary>
  /// Gets the name of the claim that was not found.
  /// </summary>
  public string ClaimName { get; }
}
