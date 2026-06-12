using System.ComponentModel.DataAnnotations;

namespace Server.Dtos.Request;

// Dados enviados pelo Agent
public record ComputerRequestDto
{
    [Required]
    [StringLength(128)]
    public string Hostname { get; init; } = string.Empty;

    [StringLength(255)]
    public string? Fqdn { get; init; }

    [StringLength(255)]
    public string? DomainName { get; init; }

    [StringLength(45)]
    public string? Ipv4Address { get; init; }

    [StringLength(17)]
    public string? MacAddress { get; init; }

    [StringLength(255)]
    public string? OperatingSystem { get; init; }

    [StringLength(50)]
    public string? AgentVersion { get; init; }

    public DateTime FirstSeenUtc { get; init; }

    public DateTime LastSeenUtc { get; init; }
}