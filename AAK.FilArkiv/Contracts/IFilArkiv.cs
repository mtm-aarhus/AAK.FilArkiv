using AAK.FilArkiv.Contracts.Models;
using AAK.FilArkiv.Features.CreateCase;
using AAK.FilArkiv.Features.CreateDocument;
using AAK.FilArkiv.Features.CreateFile;
using AAK.FilArkiv.Features.GetCaseDocumentOverview;
using AAK.FilArkiv.Features.GetFileProcessStatus;
using AAK.FilArkiv.Features.UploadFile;

namespace AAK.FilArkiv.Contracts;
public interface IFilArkiv
{
    Task<CreateCaseResponse> CreateCase(CreateCaseCommand command, CancellationToken cancellationToken = default);
    Task<CreateDocumentResponse> CreateDocument(CreateDocumentCommand command, CancellationToken cancellationToken = default);
    Task<CreateFileResponse> CreateFile(CreateFileCommand command, CancellationToken cancellationToken = default);
    Task UploadFile(UploadFileCommand command, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Document>> GetCaseDocumentOverview(GetCaseDocumentOverviewQuery query, CancellationToken cancellationToken = default);
    Task<FileProcessStatus> GetFileProcessStatus(GetFileProcessStatusQuery query, CancellationToken cancellationToken = default);
    
}
