namespace Server.Dtos.Response;

// Dados retornados pelo Server para consumo
public record FileEntryResponseDto
{
    public long Id { get; init; }

    public long ComputerId { get; init; }

    public string Path { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string Extension { get; init; } = string.Empty;

    public long SizeBytes { get; init; }

    public string Sha256 { get; init; } = string.Empty;

    public DateTime LastWriteTimeUtc { get; init; }

    public DateTime LastSeenUtc { get; init; }

    public DateTime CreatedTimeUtc { get; init; }

    public DateTime LastAccessTimeUtc { get; init; }

    public bool IsReadOnly { get; init; }

    public string Attributes { get; init; } = string.Empty;
}