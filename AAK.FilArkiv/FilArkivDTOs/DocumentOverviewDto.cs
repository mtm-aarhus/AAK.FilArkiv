using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace AAK.FilArkiv.FilArkivDTOs;

internal record DocumentOverviewDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = default!;

    [JsonPropertyName("documentNumber")]
    public int? DocumentNumber { get; set; }

    [JsonPropertyName("documentDate")]
    public DateTime? DocumentDate { get; set; }

    [JsonPropertyName("files")]
    public IReadOnlyCollection<FileDto> Files { get; set; } = new Collection<FileDto>();
}