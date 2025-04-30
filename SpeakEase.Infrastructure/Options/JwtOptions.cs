namespace SpeakEase.Infrastructure.Options;

/// <summary>
/// jwt options
/// </summary>
public sealed class JwtOptions
{
    /// <summary>
    /// 密钥
    /// </summary>
    public string SecretKey { get; set; }

    /// <summary>
    /// 颁发人
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// 受众
    /// </summary>
    public string Audience { get; set; }

    /// <summary>
    /// 过期时间(分钟)
    /// </summary>
    public int ExpMinutes { get; set; } = 30;
}