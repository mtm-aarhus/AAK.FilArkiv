namespace AAK.FilArkiv.Features.GetFileProcessStatus;

public record GetFileProcessStatusResponse(bool IsInQueue, bool IsBeingProcessed);