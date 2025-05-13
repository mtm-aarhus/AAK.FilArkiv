using System.Text.Json;
using System.Text.Json.Serialization;

namespace AAK.FilArkiv;
internal class AuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly string _authenticationUrl;
    private readonly string _clientId;
    private readonly string _clientSecret;

    private record TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccesToken { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int Expires { get; set; }
    }

    public AuthenticationService(HttpClient httpClient, string authenticationUrl, string clientId, string clientSecret)
    {
        _httpClient = httpClient;
        _authenticationUrl = authenticationUrl;
        _clientId = clientId;
        _clientSecret = clientSecret;
    }

    public async Task<string> Authenticate(CancellationToken cancellationToken = default)
    {
        if (TokenCache.Instance.ExpiresAt < DateTime.UtcNow.AddMinutes(-3)
            || string.IsNullOrWhiteSpace(TokenCache.Instance.Token))
        {
            await GetToken();
        }

        return TokenCache.Instance.Token;
    }

    private async Task GetToken(CancellationToken cancellationToken = default)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "scope", "fa_de_api:normal" },
            { "client_id",  _clientId },
            { "client_secret", _clientSecret }
        });

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            Content = content,
            RequestUri = new Uri(_authenticationUrl, UriKind.Absolute)
        };

        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);
        if (tokenResponse == null)
        {
            throw new Exception("Access token is null");
        }

        var expiration = DateTime.UtcNow.AddSeconds(tokenResponse.Expires);

        TokenCache.Instance.SetToken(tokenResponse.AccesToken);
        TokenCache.Instance.SetExpirartion(expiration);
    }
}
