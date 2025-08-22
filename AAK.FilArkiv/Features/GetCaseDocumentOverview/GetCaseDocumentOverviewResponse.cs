using AAK.FilArkiv.Contracts.Models;

namespace AAK.FilArkiv.Features.GetCaseDocumentOverview;

public record GetCaseDocumentOverviewResponse(IReadOnlyCollection<Document> Documents);