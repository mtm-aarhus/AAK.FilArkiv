using System.Text.Json.Serialization;

namespace AAK.FilArkiv.DTOs;
internal record FileDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("documentId")]
    public Guid DocumentId { get; set; }

    [JsonPropertyName("fileName")]
    public string FileName { get; set; } = default!;
}