using System.Text.Json;

namespace AAK.FilArkiv.Features.AddMatrikelToCase;

internal class AddMatrikelToCaseCommandHandler(HttpClient httpClient, AuthenticationService authenticationService)
{
    public async Task Handle(AddMatrikelToCaseCommand command, CancellationToken cancellationToken = default)
    {
        var token = await authenticationService.Authenticate(cancellationToken);
        
        var body = new
        {
            CaseId = command.CaseId,
            BasicDataType = 3,
            BasicDataId = $"{command.Ejerlavskode} {command.Matrikelnummer}"
        };

        var json = JsonSerializer.Serialize(body, Utils.SerializerOptions);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("BasicData", UriKind.Relative),
            Content = Utils.StringContent(json),
            Headers =
            {
                Authorization = Utils.AuthenticationHeader(token)
            }
        };
        
        var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}