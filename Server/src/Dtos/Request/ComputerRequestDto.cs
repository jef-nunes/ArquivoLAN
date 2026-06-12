using System.ComponentModel.DataAnnotations;

namespace Server.Dtos.Request;

public record ComputerRequestDto
{
    [Required]
    [StringLength(128)]
    public required string Hostname { get; set; }

    [StringLength(255)]
    public string? Fqdn { get; set; }

    [StringLength(255)]
    public string? DomainName { get; set; }

    [StringLength(45)]
    [Required]
    public string Ipv4Address { get; set; }

    [StringLength(17)]
    [Required]
    public required string MacAddress { get; set; }

    [StringLength(255)]
    [Required]
    public required string OperatingSystem { get; set; }
    
    [StringLength(255)]
    [Required]
    public required string AgentId { get; set; }

    [StringLength(50)]
    public string? AgentVersion { get; set; }
}