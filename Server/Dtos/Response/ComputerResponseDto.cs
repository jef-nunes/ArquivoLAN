namespace Server.Dtos.Response;

// Dados retornados pelo Server para consumo
public record ComputerResponseDto
{
    public long Id { get; init; }

    public string Hostname { get; init; } = string.Empty;

    public string? Fqdn { get; init; }

    public string? DomainName { get; init; }

    public string? Ipv4Address { get; init; }

    public string? MacAddress { get; init; }

    public string? OperatingSystem { get; init; }

    public string? AgentVersion { get; init; }

    public DateTime FirstSeenUtc { get; init; }

    public DateTime LastSeenUtc { get; init; }

    // Lista com os IDs dos FileEntry associados ao computador
    public List<long> FileEntriesIdList { get; init; } = new();
}