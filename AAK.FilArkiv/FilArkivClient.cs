using AAK.FilArkiv.Contracts;
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

namespace AAK.FilArkiv;
internal class FilArkivClient(HttpClient httpClient, AuthenticationService authenticationService) : IFilArkiv
{
    public async Task<CreateCaseResponse> CreateCase(CreateCaseCommand command, CancellationToken cancellationToken = default) => await new CreateCaseCommandHandler(httpClient, authenticationService).Handle(command, cancellationToken);
    public async Task<CreateDocumentResponse> CreateDocument(CreateDocumentCommand command, CancellationToken cancellationToken = default) => await new CreateDocumentCommandHandler(httpClient, authenticationService).Handle(command, cancellationToken);
    public async Task<CreateFileResponse> CreateFile(CreateFileCommand command, CancellationToken cancellationToken = default) => await new CreateFileCommandHandler(httpClient, authenticationService).Handle(command, cancellationToken);
    public async Task UploadFile(UploadFileCommand command, CancellationToken cancellationToken = default) => await new UploadFileCommandHandler(httpClient, authenticationService).Handle(command, cancellationToken);
    public async Task<IReadOnlyCollection<Document>> GetCaseDocumentOverview(GetCaseDocumentOverviewQuery query, CancellationToken cancellationToken = default) => await new GetCaseDocumentOverviewQueryHandler(httpClient, authenticationService).Handle(query, cancellationToken);
    public async Task<FileProcessStatus> GetFileProcessStatus(GetFileProcessStatusQuery query, CancellationToken cancellationToken = default) => await new GetFileProcessStatusQueryHandler(httpClient, authenticationService).Handle(query, cancellationToken);
    public async Task<Document?> GetDocument(GetDocumentQuery query, CancellationToken cancellationToken = default) => await new GetDocumentQueryHandler(httpClient, authenticationService).Handle(query, cancellationToken);
    public async Task<IEnumerable<File>> GetFile(GetFileQuery query, CancellationToken cancellationToken = default) => await new GetFileQueryHandler(httpClient, authenticationService).Handle(query, cancellationToken);
    public async Task AddAddressToCase(AddAddressToCaseCommand command, CancellationToken cancellationToken = default) => await new AddAddressToCaseCommandHandler(httpClient, authenticationService).Handle(command, cancellationToken);
    public async Task AddBfeToCase(AddBfeToCaseCommand command, CancellationToken cancellationToken = default) => await new AddBfeToCaseCommandHandler(httpClient, authenticationService).Handle(command, cancellationToken);
    public async Task AddMatrikelToCase(AddMatrikelToCaseCommand command, CancellationToken cancellationToken = default) => await new AddMatrikelToCaseCommandHandler(httpClient, authenticationService).Handle(command, cancellationToken);
}
