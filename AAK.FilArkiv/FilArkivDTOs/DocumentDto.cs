using System.Text.Json.Serialization;

namespace AAK.FilArkiv.FilArkivDTOs;

internal record DocumentDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }
    
    [JsonPropertyName("caseId")]
    public Guid CaseId { get; init; }
    
    [JsonPropertyName("title")]
    public string? Title { get; init; }
    
    [JsonPropertyName("documentNumber")]    
    public int? DocumentNumber { get; init; }
    
    [JsonPropertyName("documentDate")]    
    public DateTime? DocumentDate { get; init; }

    [JsonPropertyName("documentReference")]
    public string? DocumentReference { get; init; }
 
    [JsonPropertyName("files")]
    public IReadOnlyCollection<FileDto> Files { get; init; } = new List<FileDto>();
}
