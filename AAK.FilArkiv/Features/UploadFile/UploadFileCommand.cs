namespace AAK.FilArkiv.Features.UploadFile;

public record UploadFileCommand {
    public Guid FileId { get; init; }
    public string FileName { get; set; } = string.Empty;
    public Stream? Content { get; init; }
};