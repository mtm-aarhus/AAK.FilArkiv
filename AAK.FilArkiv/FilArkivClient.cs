using AAK.FilArkiv.Contracts;
using AAK.FilArkiv.Contracts.Models;
using AAK.FilArkiv.Features.CreateCase;
using AAK.FilArkiv.Features.CreateDocument;
using AAK.FilArkiv.Features.CreateFile;
using AAK.FilArkiv.Features.GetCaseDocumentOverview;
using AAK.FilArkiv.Features.GetFileProcessStatus;
using AAK.FilArkiv.Features.UploadFile;

namespace AAK.FilArkiv;
internal class FilArkivClient(HttpClient httpClient, AuthenticationService authenticationService) : IFilArkiv
{
    public async Task<CreateCaseResponse> CreateCase(CreateCaseCommand command, CancellationToken cancellationToken = default) => await new CreateCaseCommandHandler(httpClient, authenticationService).Handle(command, cancellationToken);
    public async Task<CreateDocumentResponse> CreateDocument(CreateDocumentCommand command, CancellationToken cancellationToken = default) => await new CreateDocumentCommandHandler(httpClient, authenticationService).Handle(command, cancellationToken);
    public async Task<CreateFileResponse> CreateFile(CreateFileCommand command, CancellationToken cancellationToken = default) => await new CreateFileCommandHandler(httpClient, authenticationService).Handle(command, cancellationToken);
    public async Task UploadFile(UploadFileCommand command, CancellationToken cancellationToken = default) => await new UploadFileCommandHandler(httpClient, authenticationService).Handle(command, cancellationToken);
    public async Task<IReadOnlyCollection<Document>> GetCaseDocumentOverview(GetCaseDocumentOverviewQuery query, CancellationToken cancellationToken = default) => await new GetCaseDocumentOverviewQueryHandler(httpClient, authenticationService).Handle(query, cancellationToken);
    public async Task<FileProcessStatus> GetFileProcessStatus(GetFileProcessStatusQuery query, CancellationToken cancellationToken = default) => await new GetFileProcessStatusQueryHandler(httpClient, authenticationService).Handle(query, cancellationToken);
}
