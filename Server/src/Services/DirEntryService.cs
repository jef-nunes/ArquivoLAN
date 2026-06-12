using Server.Services.Interfaces;
using Server.Models;
using Server.Persistence;

namespace Server.Services;

public class DirEntryService : IDirEntryService
{
    private readonly ArquivoLanDbContext _context;

    public DirEntryService(ArquivoLanDbContext context)
    {
        _context = context;
    }

    // ==========================================
    // CREATE
    // ==========================================
    public DirEntry Create(DirEntry dirEntry)
    {
        _context.DirEntries.Add(dirEntry);
        _context.SaveChanges();

        return dirEntry;
    }

    // ==========================================
    // FIND BY ID
    // ==========================================
    public DirEntry? FindById(long id)
    {
        return _context.DirEntries
            .FirstOrDefault(d => d.Id == id);
    }

    // ==========================================
    // FIND ALL
    // ==========================================
    public List<DirEntry> FindAll()
    {
        return _context.DirEntries.ToList();
    }

    // ==========================================
    // FIND BY SNAPSHOT ID
    // ==========================================
    public DirEntry? FindBySnapshotId(long snapshotId)
    {
        return _context.DirEntries
            .FirstOrDefault(d => d.SnapshotId == snapshotId);
    }

    // ==========================================
    // FIND BY PATH
    // ==========================================
    public DirEntry? FindByPath(string path)
    {
        return _context.DirEntries
            .FirstOrDefault(d => d.Path == path);
    }

    // ==========================================
    // UPDATE
    // ==========================================
    public DirEntry Update(long id, DirEntry dirEntry)
    {
        var existing = _context.DirEntries
            .FirstOrDefault(d => d.Id == id);

        if (existing == null)
            return null!;

        existing.Path = dirEntry.Path;
        existing.FileCount = dirEntry.FileCount;
        existing.SubdirectoryCount = dirEntry.SubdirectoryCount;
        existing.SizeBytes = dirEntry.SizeBytes;
        existing.CreatedAtUtc = dirEntry.CreatedAtUtc;
        existing.LastSeenAtUtc = DateTime.UtcNow;

        _context.SaveChanges();

        return existing;
    }

    // ==========================================
    // DELETE
    // ==========================================
    public void Delete(long id)
    {
        var entity = _context.DirEntries
            .FirstOrDefault(d => d.Id == id);

        if (entity == null)
            return;

        _context.DirEntries.Remove(entity);
        _context.SaveChanges();
    }
}