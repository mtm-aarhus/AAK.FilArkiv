using System.ComponentModel.DataAnnotations;

namespace AAK.FilArkiv.Features.CreateDocument;

public record CreateDocumentCommand
{
    [Required] public Guid CaseId { get; init; }
    [Required] public string Title { get; init; } = string.Empty;
    public int SecurityClassificationLevel { get; init; }
    public int DocumentNumber { get; init; }
    public DateTime? DocumentDate { get; init; }
    public string? DocumentReference { get; init; }
}