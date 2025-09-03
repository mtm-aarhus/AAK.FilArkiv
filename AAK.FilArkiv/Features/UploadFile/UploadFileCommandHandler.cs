namespace AAK.FilArkiv.Features.UploadFile;

internal class UploadFileCommandHandler(
    HttpClient httpClient,
    AuthenticationService authenticationService)
{
    public async Task Handle(UploadFileCommand command, CancellationToken cancellationToken)
    {
        var token = await authenticationService.Authenticate(cancellationToken);

        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(command?.Content ?? Stream.Null), "file", command.FileName);

        using var requestMessage = new HttpRequestMessage();
        requestMessage.Method = HttpMethod.Post;
        requestMessage.Content = content;
        requestMessage.RequestUri = new Uri($"FileIO/Upload/{command.FileId}", UriKind.Relative);
        requestMessage.Headers.Authorization = Utils.AuthenticationHeader(token);
        
        var response = await httpClient.SendAsync(requestMessage, cancellationToken);
        response.EnsureSuccessStatusCode();
        
    }
}