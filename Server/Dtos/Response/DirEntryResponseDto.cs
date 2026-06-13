namespace Server.Dtos.Response;

public record DirEntryResponseDto
{
    public long Id { get; set; }

    public string Path { get; set; } = string.Empty;

    public long FileCount { get; set; }

    public long SubdirectoryCount { get; set; }

    public long SizeBytes { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime LastSeenAtUtc { get; set; }

    public long SnapshotId { get; set; }
}