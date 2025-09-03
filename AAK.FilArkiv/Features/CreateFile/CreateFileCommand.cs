namespace AAK.FilArkiv.Features.CreateFile;

public record CreateFileCommand
{
    public Guid DocumentId { get; init; }
    public string FileName { get; set; } = string.Empty; 
    public string MimeType { get; set; } = string.Empty;
    public int SequenceNumber { get; set; }
    public string? FileReference { get; set; }
}