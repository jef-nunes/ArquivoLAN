using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Models;

[Table("dir_entries")]
[Index(nameof(SnapshotId), IsUnique = true)]
public class DirEntry
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }

    // ==========================================
    // RELACIONAMENTO COM SNAPSHOT (1:1)
    // ==========================================

    [Required]
    [Column("snapshot_id")]
    public long SnapshotId { get; set; }

    [ForeignKey(nameof(SnapshotId))]
    public Snapshot Snapshot { get; set; } = null!;

    // ==========================================
    // DADOS DO DIRETÓRIO ANALISADO
    // ==========================================

    [Required]
    [StringLength(1024)]
    [Column("path")]
    public string Path { get; set; } = string.Empty;

    [Column("file_count")]
    public long FileCount { get; set; }

    [Column("subdirectory_count")]
    public long SubdirectoryCount { get; set; }

    [Column("size_bytes")]
    public long SizeBytes { get; set; }

    // ==========================================
    // METADADOS TEMPORAIS
    // ==========================================

    [Column("created_at_utc")]
    public DateTime CreatedAtUtc { get; set; }

    [Column("last_seen_at_utc")]
    public DateTime LastSeenAtUtc { get; set; }
}