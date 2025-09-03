using System.Text.Json;
using AAK.FilArkiv.Contracts.Models;
using AAK.FilArkiv.FilArkivDTOs;
using Microsoft.AspNetCore.Http.Extensions;
using File = AAK.FilArkiv.Contracts.Models.File;

namespace AAK.FilArkiv.Features.GetDocument;

internal class GetDocumentQueryHandler(HttpClient httpClient, AuthenticationService authenticationService)
{
    public async Task<Document?> Handle(GetDocumentQuery query, CancellationToken cancellationToken = default)
    {
        var token = await authenticationService.Authenticate(cancellationToken);

        var queryBuilder = new QueryBuilder
        {
            { "id", query.Id.ToString() }
        };

        if (query.IncludeFiles)
        {
            queryBuilder.Add("expand", "files");
        }

        var url = string.Concat("Documents", queryBuilder.ToQueryString());
        
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
        var document = JsonSerializer.Deserialize<DocumentDto>(content, Utils.SerializerOptions);
        
        return document is null
            ? null
            : new Document
            {
                Id = document.Id,
                CaseId = document.CaseId,
                Title = document?.Title ?? string.Empty,
                DocumentDate = document?.DocumentDate,
                DocumentNumber = document?.DocumentNumber,
                DocumentReference = document?.DocumentReference,
                Files = document?.Files.Select(f => new File
                {
                    Id = f.Id,
                    DocumentId = f.DocumentId,
                    FileName = f.FileName,
                    FileSize = f.FileSize,
                }).ToList(),
            };
    }
}