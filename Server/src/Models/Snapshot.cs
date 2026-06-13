using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("snapshots")]
public class Snapshot
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("started_at_utc")]
    public DateTime StartedAtUtc { get; set; }

    [Column("finished_at_utc")]
    public DateTime? FinishedAtUtc { get; set; }

    [Required]
    [StringLength(1024)]
    [Column("target_path")]
    public string TargetPath { get; set; } = string.Empty;

    [Required]
    [Column("computer_id")]
    public long ComputerId { get; set; }

    [ForeignKey(nameof(ComputerId))]
    public Computer Computer { get; set; } = null!;

    // 1 Snapshot → N FileEntry
    public ICollection<FileEntry> FileEntries { get; set; }
        = new List<FileEntry>();

    // 1 Snapshot → N DirEntry
    public ICollection<DirEntry> DirEntries { get; set; }
    = new List<DirEntry>();
}