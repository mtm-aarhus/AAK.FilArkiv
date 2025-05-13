namespace AAK.FilArkiv;
internal sealed class TokenCache
{
    private static readonly Lazy<TokenCache> _instance = new(() => new());
    public static TokenCache Instance => _instance.Value;

    public string Token { get; private set; } = default!;
    public DateTime ExpiresAt { get; set; } = DateTime.MinValue;

    public void SetToken(string token) => Token = token;
    public void SetExpirartion(DateTime expirartion) => ExpiresAt = expirartion;
}
