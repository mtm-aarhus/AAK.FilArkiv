using System.Text.Json;
using AAK.FilArkiv.FilArkivDTOs;

namespace AAK.FilArkiv.Features.CreateDocument;

internal class CreateDocumentCommandHandler(
    HttpClient httpClient,
    AuthenticationService authenticationService
    )
{
    public async Task<CreateDocumentResponse> Handle(CreateDocumentCommand command, CancellationToken cancellationToken = default)
    {
        var token = await authenticationService.Authenticate(cancellationToken);

        var body = new
        {
            CaseId = command.CaseId,
            Title = command.Title,
            SecurityClassificationLevel = command.SecurityClassificationLevel,
            DocumentNumber = command.DocumentNumber,
            DocumentDate = command.DocumentDate,
            DocumentReference = command.DocumentReference,
        };

        var json = JsonSerializer.Serialize(body, Utils.SerializerOptions);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("Documents", UriKind.Relative),
            Content = Utils.StringContent(json),
            Headers =
            {
                Authorization = Utils.AuthenticationHeader(token)
            }
        };
        
        var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        var responseDto = JsonSerializer.Deserialize<DocumentDto>(responseBody, Utils.SerializerOptions);
        return new CreateDocumentResponse(responseDto?.Id ?? Guid.Empty);
    }
}