namespace Entities.Common;

/// <summary>
/// Class <c>Common</c> contains the constant values used throughout the application.
/// </summary>
public static class Common
{
    /// <summary>
    /// The jwt key string
    /// </summary>
    public static readonly string TOKEN_KEY = "Jwt:Key";

    /// <summary>
    /// The jwt issuer string
    /// </summary>
    public static readonly string TOKEN_ISSUER = "Jwt:Issuer";

    /// <summary>
    /// The bearer string
    /// </summary>
    public static readonly string BEARER_TOKEN = "Bearer";

    /// <summary>
    /// The deafult user password
    /// </summary>
    public static readonly string DEFAULT_PASSWORD = "welcome1234";
}
