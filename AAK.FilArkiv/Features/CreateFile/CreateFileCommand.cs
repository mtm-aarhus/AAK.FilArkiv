using System.ComponentModel.DataAnnotations;

namespace AAK.FilArkiv.Features.CreateFile;

public record CreateFileCommand
{
    [Required] public Guid DocumentId { get; init; }
    [Required] public string FileName { get; set; } = string.Empty;
    [Required] public string MimeType { get; set; } = string.Empty;
    public int SequenceNumber { get; set; }
    public string? FileReference { get; set; }
}