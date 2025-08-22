using System.Text.Json;
using AAK.FilArkiv.FilArkivDTOs;

namespace AAK.FilArkiv.Features.CreateFile;

internal class CreateFileCommandHandler(
    HttpClient httpClient,
    AuthenticationService authenticationService)
{
    public async Task<CreateFileResponse> Handle(CreateFileCommand command, CancellationToken cancellationToken = default)
    {
        var token = await authenticationService.Authenticate(cancellationToken);

        var body = new
        {
            DocumentId = command.DocumentId,
            FileName = command.FileName,
            MimeType = command.MimeType,
            SequenceNumber = command.SequenceNumber,
            FileReference = command.FileReference
        };
        
        var json = JsonSerializer.Serialize(body, Utils.SerializerOptions);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("files", UriKind.Relative),
            Content = Utils.StringContent(json),
            Headers =
            {
                Authorization = Utils.AuthenticationHeader(token)
            }
        };
        
        var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        var responseDto = JsonSerializer.Deserialize<FileDto>(responseBody, Utils.SerializerOptions);

        return new CreateFileResponse(responseDto?.Id ?? Guid.Empty);
    }
}