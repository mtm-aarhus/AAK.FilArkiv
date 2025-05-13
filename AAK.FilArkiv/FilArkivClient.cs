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

    public FilArkivClient(HttpClient httpClient, AuthenticationService authenticationService)
    {
        _httpClient = httpClient;
        _authenticationService = authenticationService;
    }

    public async Task<IReadOnlyCollection<Document>> GetCaseDocumentOverview(Guid caseId, CancellationToken cancellationToken = default)
    {
        int pageIndex = 1; // First page = pageIndex = 1
        var documents = new Collection<Document>();

        while (true)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"/Documents/CaseDocumentOverview?caseId={caseId}&pageSize=100&pageIndex={pageIndex}", UriKind.Relative)
            };

            var token = await _authenticationService.Authenticate(cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var overviewDto = JsonSerializer.Deserialize<IEnumerable<DocumentOverviewDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            
            if (overviewDto is null || overviewDto.Count() == 0)
            {
                break;
            }

            foreach (var dto in overviewDto)
            {
                documents.Add(new Document
                {
                    Id = dto.Id,
                });
            }
            
            pageIndex++;
        }

        return documents;
    }
}
