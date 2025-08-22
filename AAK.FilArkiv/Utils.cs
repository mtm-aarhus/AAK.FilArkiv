using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AAK.FilArkiv;

internal static class Utils
{
    public static JsonSerializerOptions SerializerOptions => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static AuthenticationHeaderValue AuthenticationHeader(string token) => new("Bearer", token);
    public static StringContent StringContent(string content) => new(content, Encoding.UTF8, "application/json");
}