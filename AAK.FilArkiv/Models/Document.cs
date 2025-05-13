using System.Collections.ObjectModel;

namespace AAK.FilArkiv.Models;
public record Document
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public int? DocumentNumber { get; set; }
    public DateTime? DocumentDate { get; set; }
    public IReadOnlyCollection<File> Files { get; set; } = new Collection<File>();
}
