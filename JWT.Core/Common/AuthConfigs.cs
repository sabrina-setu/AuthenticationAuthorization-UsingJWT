namespace JWT.Core.Common;

public class AuthConfigs
{
    public string JwtSecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpireTime { get; set; }
}
