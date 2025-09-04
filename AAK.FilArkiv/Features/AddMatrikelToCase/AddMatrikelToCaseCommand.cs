namespace AAK.FilArkiv.Features.AddMatrikelToCase;

public record AddMatrikelToCaseCommand(Guid CaseId, int Ejerlavskode, string Matrikelnummer);