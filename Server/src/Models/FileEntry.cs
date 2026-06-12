using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("file_entries")]
public class FileEntry
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }

    // ==========================================
    // NOVO RELACIONAMENTO (Snapshot)
    // ==========================================

    [Required]
    [Column("snapshot_id")]
    public long SnapshotId { get; set; }

    [ForeignKey(nameof(SnapshotId))]
    public Snapshot Snapshot { get; set; } = null!;

    // ==========================================
    // DADOS DO ARQUIVO
    // ==========================================

    [Required]
    [StringLength(1024)]
    [Column("path")]
    public string Path { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(32)]
    [Column("extension")]
    public string Extension { get; set; } = string.Empty;

    [Column("size_bytes")]
    public long SizeBytes { get; set; }

    [Required]
    [StringLength(64)]
    [Column("sha256")]
    public string Sha256 { get; set; } = string.Empty;

    [Column("last_write_time_utc")]
    public DateTime LastWriteTimeUtc { get; set; }

    [Column("last_seen_utc")]
    public DateTime LastSeenUtc { get; set; }

    [Column("created_time_utc")]
    public DateTime CreatedTimeUtc { get; set; }

    [Column("last_access_time_utc")]
    public DateTime LastAccessTimeUtc { get; set; }

    [Column("is_read_only")]
    public bool IsReadOnly { get; set; }

    [Required]
    [StringLength(256)]
    [Column("attributes")]
    public string Attributes { get; set; } = string.Empty;
}