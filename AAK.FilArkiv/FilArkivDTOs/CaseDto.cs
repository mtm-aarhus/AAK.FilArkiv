namespace AAK.FilArkiv.FilArkivDTOs;

public record CaseDto
{ 
    public Guid Id { get; init; }
    public Guid ArchiveId { get; init; }
}