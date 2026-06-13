namespace Server.Dtos.Response;

public record FileEntryResponseDto
{
    public long Id { get; set; }

    public string Path { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Extension { get; set; } = string.Empty;

    public long SizeBytes { get; set; }

    public string Sha256 { get; set; } = string.Empty;

    public DateTime LastWriteTimeUtc { get; set; }

    public DateTime LastSeenUtc { get; set; }

    public DateTime CreatedTimeUtc { get; set; }

    public DateTime LastAccessTimeUtc { get; set; }

    public bool IsReadOnly { get; set; }

    public string Attributes { get; set; } = string.Empty;
    
    public long SnapshotId { get; set; }
}