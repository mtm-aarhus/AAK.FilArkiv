﻿namespace AAK.FilArkiv.Models;
public record File
{
    public Guid Id { get; set; }
    public Guid DocumentId { get; set; }
    public string FileName { get; set; } = default!;
}
