namespace AAK.FilArkiv.Features.CreateCase;

public record CreateCaseCommand
{
    public string CaseNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? CaseReference { get; set; }
    public DateTime? CaseDate { get; set; }
    public Guid CaseTypeId { get; set; }
    public Guid CaseStatusId { get; set; }
    public Guid ArchiveId { get; set; }
    public int SecurityClassificationLevel { get; set; }
}