using System.ComponentModel.DataAnnotations;

namespace Server.Dtos.Request;

// Dados enviados pelo Agent
public record FileEntryRequestDto
{
    [Required]
    [StringLength(1024)]
    public string Path { get; init; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [StringLength(32)]
    public string Extension { get; init; } = string.Empty;

    [Range(0, long.MaxValue)]
    public long SizeBytes { get; init; }

    [Required]
    [StringLength(64)]
    public string Sha256 { get; init; } = string.Empty;

    public DateTime LastWriteTimeUtc { get; init; }

    public DateTime LastSeenTimeUtc { get; init; }

    public DateTime CreatedTimeUtc { get; init; }

    public DateTime LastAccessTimeUtc { get; init; }

    public bool IsReadOnly { get; init; }

    [Required]
    [StringLength(256)]
    public string Attributes { get; init; } = string.Empty;
}