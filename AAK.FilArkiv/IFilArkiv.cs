using AAK.FilArkiv.Models;

namespace AAK.FilArkiv;
public interface IFilArkiv
{
    Task<IReadOnlyCollection<Document>> GetCaseDocumentOverview(Guid caseId, CancellationToken cancellationToken = default);
    Task<FileProcessStatus> GetFileProcessStatus(Guid fileId, CancellationToken cancellationToken = default);
}
