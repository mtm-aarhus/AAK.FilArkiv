using System.Text.Json.Serialization;

namespace AAK.FilArkiv.FilArkivDTOs;

internal record FileDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("documentId")]
    public Guid DocumentId { get; init; }

    [JsonPropertyName("fileName")]
    public string FileName { get; init; } =  string.Empty;
    
    [JsonPropertyName("fileSize")]
    public long FileSize { get; init; }
}