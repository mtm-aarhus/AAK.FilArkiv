using System.Collections.ObjectModel;
using File = AAK.FilArkiv.Contracts.Models.File;

namespace AAK.FilArkiv.Contracts.Models;
public record Document
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public int? DocumentNumber { get; set; }
    public DateTime? DocumentDate { get; set; }
    public IReadOnlyCollection<File> Files { get; set; } = new Collection<File>();
}
