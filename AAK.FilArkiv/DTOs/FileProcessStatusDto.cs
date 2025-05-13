using System.Text.Json.Serialization;

namespace AAK.FilArkiv.DTOs;
internal record FileProcessStatusDto
{
    [JsonPropertyName("isInQueue")]
    public bool IsInQueue { get; set; }

    [JsonPropertyName("isBeingProcessed")]
    public bool IsBeingProcessed { get; set; }
}
