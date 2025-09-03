using System.Text.Json;
using AAK.FilArkiv.Contracts.Models;
using AAK.FilArkiv.FilArkivDTOs;
using Microsoft.AspNetCore.Http.Extensions;

namespace AAK.FilArkiv.Features.GetFileProcessStatus;

internal class GetFileProcessStatusQueryHandler(
    HttpClient httpClient,
    AuthenticationService authenticationService)
{
    public async Task<FileProcessStatus> Handle(GetFileProcessStatusQuery query, CancellationToken cancellationToken = default)
    {
        var token = await authenticationService.Authenticate(cancellationToken);

        var queryBuilder = new QueryBuilder
        {
            { "fileId", query.FileId.ToString() }
        };
        
        var url = string.Concat("FileProcess/FileProcessStatusFile", queryBuilder.ToQueryString());
        
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
        var dto = JsonSerializer.Deserialize<FileProcessStatusDto>(content, Utils.SerializerOptions);

        if (dto is null)
        {
            throw new Exception("FileProcessStatus FilArkiv response is null");
        }

        return new FileProcessStatus(dto.IsInQueue, dto.IsBeingProcessed);
    }
}