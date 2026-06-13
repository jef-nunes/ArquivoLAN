using System.ComponentModel.DataAnnotations;

namespace Contracts.Dtos.Request;

public record DirEntryRequestDto
{
    [Required]
    [StringLength(1024)]
    public string Path { get; set; } = string.Empty;

    public long FileCount { get; set; }

    public long SubdirectoryCount { get; set; }

    public long SizeBytes { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime LastSeenAtUtc { get; set; }
}