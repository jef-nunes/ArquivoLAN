namespace Server.Dtos.Response;

public record SnapshotResponseDto
{
    public long Id { get; set; }

    public DateTime StartedAtUtc { get; set; }

    public DateTime? FinishedAtUtc { get; set; }

    public string TargetPath { get; set; } = string.Empty;

    public long ComputerId { get; set; }

    public long DirEntryId { get; set; }

    public List<long> FileEntryIdList = new List<long>();
}