using System.Net;
using AAK.FilArkiv.Contracts.Models;
using AAK.FilArkiv.Features.AddAddressToCase;
using AAK.FilArkiv.Features.AddBfeToCase;
using AAK.FilArkiv.Features.AddMatrikelToCase;
using AAK.FilArkiv.Features.CreateCase;
using AAK.FilArkiv.Features.CreateDocument;
using AAK.FilArkiv.Features.CreateFile;
using AAK.FilArkiv.Features.GetCaseDocumentOverview;
using AAK.FilArkiv.Features.GetDocument;
using AAK.FilArkiv.Features.GetFile;
using AAK.FilArkiv.Features.GetFileProcessStatus;
using AAK.FilArkiv.Features.UploadFile;
using File = AAK.FilArkiv.Contracts.Models.File;

namespace AAK.FilArkiv.Contracts;
public interface IFilArkiv
{
    Task<CreateCaseResponse> CreateCase(CreateCaseCommand command, CancellationToken cancellationToken = default);
    Task<CreateDocumentResponse> CreateDocument(CreateDocumentCommand command, CancellationToken cancellationToken = default);
    Task<CreateFileResponse> CreateFile(CreateFileCommand command, CancellationToken cancellationToken = default);
    Task UploadFile(UploadFileCommand command, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Document>> GetCaseDocumentOverview(GetCaseDocumentOverviewQuery query, CancellationToken cancellationToken = default);
    Task<FileProcessStatus> GetFileProcessStatus(GetFileProcessStatusQuery query, CancellationToken cancellationToken = default);
    Task<Document?> GetDocument(GetDocumentQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<File>> GetFile(GetFileQuery query, CancellationToken cancellationToken = default);
    Task AddAddressToCase(AddAddressToCaseCommand command, CancellationToken cancellationToken = default);
    Task AddBfeToCase(AddBfeToCaseCommand command, CancellationToken cancellationToken = default);
    Task AddMatrikelToCase(AddMatrikelToCaseCommand command, CancellationToken cancellationToken = default);
}
