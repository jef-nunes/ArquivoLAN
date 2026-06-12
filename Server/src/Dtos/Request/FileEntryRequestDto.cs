using System.ComponentModel.DataAnnotations;

namespace Server.Dtos.Request;

public record FileEntryRequestDto
{
    [Required]
    [StringLength(1024)]
    public string Path { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(32)]
    public string Extension { get; set; } = string.Empty;

    public long SizeBytes { get; set; }

    [Required]
    [StringLength(64)]
    public string Sha256 { get; set; } = string.Empty;

    public DateTime LastWriteTimeUtc { get; set; }

    public DateTime LastSeenUtc { get; set; }

    public DateTime CreatedTimeUtc { get; set; }

    public DateTime LastAccessTimeUtc { get; set; }

    public bool IsReadOnly { get; set; }

    [StringLength(256)]
    public string Attributes { get; set; } = string.Empty;
}