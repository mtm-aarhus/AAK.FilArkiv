namespace AAK.FilArkiv.Features.AddAddressToCase;

public record AddAddressToCaseCommand(Guid CaseId, Guid AddressId);