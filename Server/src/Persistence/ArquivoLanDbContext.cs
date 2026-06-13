using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Persistence;

public class ArquivoLanDbContext : DbContext
{
    public ArquivoLanDbContext(DbContextOptions<ArquivoLanDbContext> options) : base(options)
    { 
    }
    
    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Computer 1:N Snapshot
        modelBuilder.Entity<Snapshot>()
            .HasOne(s => s.Computer)
            .WithMany(c => c.Snapshots)
            .HasForeignKey(s => s.ComputerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Snapshot 1:N FileEntry
        modelBuilder.Entity<FileEntry>()
            .HasOne(f => f.Snapshot)
            .WithMany(s => s.FileEntries)
            .HasForeignKey(f => f.SnapshotId)
            .OnDelete(DeleteBehavior.Cascade);

        // Snapshot 1:1 DirEntry
        modelBuilder.Entity<DirEntry>()
            .HasOne(d => d.Snapshot)
            .WithMany(s => s.DirEntries)
            .HasForeignKey(d => d.SnapshotId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    public DbSet<Computer> Computers { get; set; }
    public DbSet<Snapshot> Snapshots { get; set; }
    public DbSet<DirEntry> DirEntries { get; set; }
    public DbSet<FileEntry> FileEntries { get; set; }
}