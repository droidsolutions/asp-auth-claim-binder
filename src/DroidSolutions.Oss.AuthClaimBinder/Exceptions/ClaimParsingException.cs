using System.Runtime.Serialization;

namespace DroidSolutions.Oss.AuthClaimBinder.Exceptions;

/// <summary>
/// Special exception when a claim can not be parsed to the destination type.
/// </summary>
[Serializable]
public class ClaimParsingException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ClaimParsingException"/> class.
  /// </summary>
  public ClaimParsingException()
  {
    ClaimName = string.Empty;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ClaimParsingException"/> class.
  /// </summary>
  /// <param name="name">The name of the claim.</param>
  /// <param name="type">The type that the claim should have been parsed to.</param>
  public ClaimParsingException(string name, Type? type)
    : this($"The claim \"{name}\" was not found.", null, name, type)
  { }

  /// <summary>
  /// Initializes a new instance of the <see cref="ClaimParsingException"/> class.
  /// </summary>
  /// <param name="message">The message of the exception.</param>
  public ClaimParsingException(string? message)
    : this(message, (Exception?)null)
  { }

  /// <summary>
  /// Initializes a new instance of the <see cref="ClaimParsingException"/> class.
  /// </summary>
  /// <param name="message">The exception message.</param>
  /// <param name="name">The name of the claim.</param>
  /// <param name="type">The type that the claim should have been parsed to.</param>
  public ClaimParsingException(string message, string name, Type? type)
    : this(message, null, name, type)
  { }

  /// <summary>
  /// Initializes a new instance of the <see cref="ClaimParsingException"/> class.
  /// </summary>
  /// <param name="message">The exception message.</param>
  /// <param name="inner">The inner exception, if any.</param>
  public ClaimParsingException(string? message, Exception? inner)
    : base(message, inner)
  {
    ClaimName = string.Empty;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ClaimParsingException"/> class.
  /// </summary>
  /// <param name="message">The exception message.</param>
  /// <param name="inner">The inner exception, if any.</param>
  /// <param name="name">The name of the claim.</param>
  /// <param name="type">The type that the claim should have been parsed to.</param>
  public ClaimParsingException(string? message, Exception? inner, string name, Type? type)
    : base(message, inner)
  {
    ClaimName = name;
    ClaimType = type;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ClaimParsingException"/> class.
  /// </summary>
  /// <param name="info">Runtime serialization info.</param>
  /// <param name="context">Streaming context for serialization.</param>
  protected ClaimParsingException(
    SerializationInfo info,
    StreamingContext context)
    : base(info, context)
  {
    ClaimName = info.GetString(nameof(ClaimName)) ?? string.Empty;

    string? typeName = info.GetString(nameof(ClaimType));
    if (!string.IsNullOrEmpty(typeName))
    {
      ClaimType = Type.GetType(typeName);
    }
  }

  /// <summary>
  /// Gets the name of the claim that could not be parsed.
  /// </summary>
  public string ClaimName { get; }

  /// <summary>
  /// Gets the type the claim should have been parsed to.
  /// </summary>
  public Type? ClaimType { get; }
}
