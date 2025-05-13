using AAK.FilArkiv.DTOs;
using AAK.FilArkiv.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AAK.FilArkiv;
internal class FilArkivClient : IFilArkiv
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationService _authenticationService;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

    public FilArkivClient(HttpClient httpClient, AuthenticationService authenticationService)
    {
        _httpClient = httpClient;
        _authenticationService = authenticationService;
    }

    public async Task<IReadOnlyCollection<Document>> GetCaseDocumentOverview(Guid caseId, CancellationToken cancellationToken = default)
    {
        int pageIndex = 1; // First page = pageIndex = 1
        var pageSize = 50;
        var documents = new Collection<Document>();

        while (true)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"Documents/CaseDocumentOverview?caseId={caseId}&pageSize={pageSize}&pageIndex={pageIndex}", UriKind.Relative)
            };

            var token = await _authenticationService.Authenticate(cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            // Header responses
            var headers = response.Headers;
            var hasNextPageHeaderValue = headers.FirstOrDefault(x => x.Key == "Pagination-HasNextPage").Value;
            var hasNextPage = hasNextPageHeaderValue?.FirstOrDefault()?.ToLowerInvariant() == "true" ? true : false;

            var overviewDto = JsonSerializer.Deserialize<IEnumerable<DocumentOverviewDto>>(content, _jsonSerializerOptions);
            
            // Exit loop if there is no documents in the response
            if (overviewDto is null || overviewDto.Count() == 0)
            {
                break;
            }

            // Map DTO to model
            foreach (var dto in overviewDto)
            {
                documents.Add(new Document
                {
                    Id = dto.Id,
                    DocumentDate = dto.DocumentDate,
                    DocumentNumber = dto.DocumentNumber,
                    Title = dto.Title,
                    Files = [.. dto.FileDtos.Select(f => new Models.File
                    {
                        Id = f.Id,
                        DocumentId = f.Id,
                        FileName = f.FileName
                    })]
                });
            }

            // Exit loop if there is not a next page
            if (!hasNextPage)
            {
                break;
            }

            pageIndex++;

        }

        return documents;
    }

    public async Task<FileProcessStatus> GetFileProcessStatus(Guid fileId, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"FileProcess/FileProcessStatusFile?fileId={fileId}", UriKind.Relative)
        };

        var token = await _authenticationService.Authenticate(cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var dto = JsonSerializer.Deserialize<FileProcessStatusDto>(content, _jsonSerializerOptions);

        if (dto is null)
        {
            throw new Exception("FileProcessStatus FilArkiv response is null");
        }

        return new FileProcessStatus(dto.IsInQueue, dto.IsBeingProcessed);
    }
}
