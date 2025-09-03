namespace AAK.FilArkiv.Contracts.Models;
public record Document
{
    public Guid Id { get; init; }
    public Guid CaseId { get; init; }
    public string Title { get; set; } = string.Empty;
    public int? DocumentNumber { get; set; }
    public DateTime? DocumentDate { get; set; }
    public string? DocumentReference { get; init; }
    public IReadOnlyCollection<File>? Files { get; set; } = new List<File>();
}