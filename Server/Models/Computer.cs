using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

// Representa um computador na rede local
[Table("computers")]
public class Computer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [StringLength(128)]
    [Column("hostname")]
    public string Hostname { get; set; } = string.Empty;

    [StringLength(255)]
    [Column("fqdn")]
    public string? Fqdn { get; set; }

    [StringLength(255)]
    [Column("domain_name")]
    public string? DomainName { get; set; }

    [StringLength(45)]
    [Column("ipv4_address")]
    public string? Ipv4Address { get; set; }

    [StringLength(17)]
    [Column("mac_address")]
    public string? MacAddress { get; set; }

    [StringLength(255)]
    [Column("operating_system")]
    public string? OperatingSystem { get; set; }

    [StringLength(50)]
    [Column("agent_version")]
    public string? AgentVersion { get; set; }

    [Column("first_seen_utc")]
    public DateTime FirstSeenUtc { get; set; }

    [Column("last_seen_utc")]
    public DateTime LastSeenUtc { get; set; }

    // Relacionamento com o modelo FileEntry
    // Um computador pode ter vários file entries
    public ICollection<FileEntry> FileEntries { get; set; }
        = new List<FileEntry>();
}