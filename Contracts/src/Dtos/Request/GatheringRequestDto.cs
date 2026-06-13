using System.ComponentModel.DataAnnotations;

namespace Contracts.Dtos.Request;

public class GatheringRequestDto
{
    [Required]
    public required ComputerRequestDto Computer { get; set; }

    [Required]
    public required SnapshotRequestDto Snapshot { get; set; }

    [Required]
    public required List<DirEntryRequestDto> DirEntries { get; set; }
        = new();

    [Required]
    public required List<FileEntryRequestDto> FileEntries { get; set; }
        = new();
}