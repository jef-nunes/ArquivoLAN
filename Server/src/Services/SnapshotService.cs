using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Persistence;
using Server.Services.Interfaces;

namespace Server.Services;

public class SnapshotService : ISnapshotService
{
    private readonly ArquivoLanDbContext _context;

    public SnapshotService(ArquivoLanDbContext context)
    {
        _context = context;
    }
    
    // ==========================================
    // CREATE (ISnapshotService)
    // ==========================================
    public Snapshot Create(Snapshot snapshot, long computerId)
    {
        var computer = _context.Computers
            .FirstOrDefault(c => c.Id == computerId);

        if (computer == null)
            return null!;

        snapshot.ComputerId = computer.Id;
        snapshot.StartedAtUtc = DateTime.UtcNow;

        _context.Snapshots.Add(snapshot);
        _context.SaveChanges();

        return snapshot;
    }

    // ==========================================
    // FIND BY ID
    // ==========================================
    public Snapshot? FindById(long id)
    {
        return _context.Snapshots
            .Include(s => s.FileEntries)
            .Include(s => s.DirEntry)
            .FirstOrDefault(s => s.Id == id);
    }

    // ==========================================
    // FIND ALL
    // ==========================================
    public List<Snapshot> FindAll()
    {
        return _context.Snapshots
            .Include(s => s.FileEntries)
            .Include(s => s.DirEntry)
            .ToList();
    }

    // ==========================================
    // FIND BY TIME RANGE
    // ==========================================
    public List<Snapshot>? FindAllByTimeRange(DateTime from, DateTime to)
    {
        return _context.Snapshots
            .Where(s => s.StartedAtUtc >= from && s.StartedAtUtc <= to)
            .Include(s => s.FileEntries)
            .Include(s => s.DirEntry)
            .ToList();
    }

    // ==========================================
    // UPDATE
    // ==========================================
    public Snapshot Update(long id, Snapshot snapshot)
    {
        var existing = _context.Snapshots
            .FirstOrDefault(s => s.Id == id);

        if (existing == null)
            return null!;

        existing.FinishedAtUtc = snapshot.FinishedAtUtc;
        existing.TargetPath = snapshot.TargetPath;

        _context.SaveChanges();

        return existing;
    }

    // ==========================================
    // DELETE
    // ==========================================
    public void Delete(long id)
    {
        var entity = _context.Snapshots
            .Include(s => s.FileEntries)
            .Include(s => s.DirEntry)
            .FirstOrDefault(s => s.Id == id);

        if (entity == null)
            return;

        _context.Snapshots.Remove(entity);
        _context.SaveChanges();
    }
}