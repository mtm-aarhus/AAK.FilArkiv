using System.Net.Http.Headers;
using System.Text.Json;
using AAK.FilArkiv.Contracts.Models;
using AAK.FilArkiv.FilArkivDTOs;
using File = AAK.FilArkiv.Contracts.Models.File;

namespace AAK.FilArkiv.Features.GetCaseDocumentOverview;

internal class GetCaseDocumentOverviewQueryHandler(HttpClient httpClient, AuthenticationService authenticationService)
{
    public async Task<IReadOnlyCollection<Document>> Handle(GetCaseDocumentOverviewQuery query, CancellationToken cancellationToken = default)
    {
        var pageIndex = 1; // First page = pageIndex = 1
        var pageSize = 50;
        var documents = new List<Document>();

        while (true)
        {
            var token = await authenticationService.Authenticate(cancellationToken);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"Documents/CaseDocumentOverview?caseId={query.CaseId}&pageSize={pageSize}&pageIndex={pageIndex}", UriKind.Relative),
                Headers =
                {
                    Authorization = new AuthenticationHeaderValue("Bearer", token)
                }
            };

            var response = await httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            // Header responses
            var headers = response.Headers;
            var hasNextPageHeaderValue = headers.FirstOrDefault(x => x.Key == "Pagination-HasNextPage").Value;
            var hasNextPage = hasNextPageHeaderValue?.FirstOrDefault()?.ToLowerInvariant() == "true";

            var overviewDto = JsonSerializer.Deserialize<DocumentOverviewDto[]>(content, Utils.SerializerOptions);
            
            // Exit loop if there is no documents in the response
            if (overviewDto is null || overviewDto.Length == 0)
            {
                break;
            }

            // Map DTO to model
            documents.AddRange(overviewDto.Select(dto => new Document
            {
                Id = dto.Id,
                DocumentDate = dto.DocumentDate,
                DocumentNumber = dto.DocumentNumber,
                Title = dto.Title,
                Files = [.. dto.Files.Select(f => new File { Id = f.Id, DocumentId = f.Id, FileName = f.FileName })]
            }));

            // Exit loop if there is no nore pages
            if (!hasNextPage)
            {
                break;
            }

            pageIndex++;
        }

        return documents;
    }
}