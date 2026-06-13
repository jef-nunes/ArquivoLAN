namespace Server.Dtos.Response;

public record ComputerResponseDto
{
    public long Id { get; set; }

    public required string Hostname { get; set; } = string.Empty;

    public string? Fqdn { get; set; }

    public string? DomainName { get; set; }

    public string? Ipv4Address { get; set; }

    public required string MacAddress { get; set; }

    public required string OperatingSystem { get; set; }
    
    public required string AgentId { get; set; }

    public string? AgentVersion { get; set; }

    public DateTime FirstSeenUtc { get; set; }

    public DateTime LastSeenUtc { get; set; }

    public List<long> SnapshotIdList { get; set; } = new List<long>();
}