using System.ComponentModel.DataAnnotations;

namespace Contracts.Dtos.Request;

public record SnapshotRequestDto
{
    [Required]
    public DateTime StartedAtUtc { get; set; }

    public DateTime? FinishedAtUtc { get; set; }

    [Required]
    [StringLength(1024)]
    public string TargetPath { get; set; } = string.Empty;
}