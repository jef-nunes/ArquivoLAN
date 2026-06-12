using System.ComponentModel.DataAnnotations;

namespace Server.Dtos.Request;

public class GatheringRequestDto
{
    [Required]
    public required ComputerRequestDto Computer { get; set; }

    [Required]
    public required SnapshotRequestDto Snapshot { get; set; }

    [Required]
    public required DirEntryRequestDto DirEntry { get; set; }

    [Required]
    public required List<FileEntryRequestDto> FileEntries { get; set; }
        = new();
}