using System.Text.Json;

namespace AAK.FilArkiv.Features.AddAddressToCase;

internal class AddAddressToCaseCommandHandler(HttpClient httpClient, AuthenticationService authenticationService)
{
    public async Task Handle(AddAddressToCaseCommand command, CancellationToken cancellationToken = default)
    {
        var token = await authenticationService.Authenticate(cancellationToken);
        
        var body = new
        {
            CaseId = command.CaseId,
            BasicDataType = 1,
            BasicDataId = command.AddressId
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