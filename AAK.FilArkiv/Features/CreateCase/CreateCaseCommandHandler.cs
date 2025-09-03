using System.Text.Json;
using AAK.FilArkiv.FilArkivDTOs;

namespace AAK.FilArkiv.Features.CreateCase;

internal class CreateCaseCommandHandler(HttpClient httpClient, AuthenticationService authenticationService)
{
    public async Task<CreateCaseResponse> Handle(CreateCaseCommand command, CancellationToken cancellationToken = default)
    {
        var token = await authenticationService.Authenticate(cancellationToken);

        var body = new
        {
            CaseNumber = command.CaseNumber,
            Title = command.Title,
            CaseReference = command.CaseReference,
            CaseDate = command.CaseDate,
            CaseTypeId = command.CaseTypeId,
            CaseStatusId = command.CaseStatusId,
            ArchiveId = command.ArchiveId,
            SecurityClassificationLevel = command.SecurityClassificationLevel,
        };

        var json = JsonSerializer.Serialize(body, Utils.SerializerOptions);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            Content = Utils.StringContent(json),
            RequestUri = new Uri("Cases", UriKind.Relative),
            Headers =
            {
                Authorization = Utils.AuthenticationHeader(token)
            }
        };
            
        var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        var responseObject = JsonSerializer.Deserialize<CaseDto>(responseBody, Utils.SerializerOptions);

        var dto = new CreateCaseResponse(
            CaseId: responseObject?.Id ?? Guid.Empty,
            ArchiveId: responseObject?.ArchiveId ?? Guid.Empty);
        
        return dto;
    }
}