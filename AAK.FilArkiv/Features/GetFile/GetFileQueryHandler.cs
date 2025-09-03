using System.Text.Json;
using AAK.FilArkiv.FilArkivDTOs;
using Microsoft.AspNetCore.Http.Extensions;

namespace AAK.FilArkiv.Features.GetFile;

internal class GetFileQueryHandler(HttpClient httpClient, AuthenticationService authenticationService)
{
    public async Task<IEnumerable<AAK.FilArkiv.Contracts.Models.File>> Handle(GetFileQuery query, CancellationToken cancellationToken)
    {
        var token = await authenticationService.Authenticate(cancellationToken);
        var queryBuilder = new QueryBuilder
        {
            { "id", query.FileId.ToString() }
        };
        
        var url = string.Concat("Files", queryBuilder.ToQueryString());
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url, UriKind.Relative),
            Headers =
            {
                Authorization = Utils.AuthenticationHeader(token)
            }
        };
        
        var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var files = JsonSerializer.Deserialize<IEnumerable<FileDto>>(content, Utils.SerializerOptions);
        
        return files?.Select(x => new AAK.FilArkiv.Contracts.Models.File
        {
            Id = x.Id,
            DocumentId = x.DocumentId,
            FileName = x.FileName,
            FileSize = x.FileSize
        }) ?? [];
    }
}