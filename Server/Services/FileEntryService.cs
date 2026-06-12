using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Persistence;

namespace Server.Services;

public class FileEntryService : IFileEntryService
{
    private readonly ArquivoLanDbContext _context;

    public FileEntryService(ArquivoLanDbContext context)
    {
        _context = context;
    }

    public FileEntry Create(FileEntry fileEntry)
    {
        _context.FileEntries.Add(fileEntry);
        _context.SaveChanges();

        return fileEntry;
    }

    public FileEntry? FindById(long id)
    {
        return _context.FileEntries
            .FirstOrDefault(fe => fe.Id == id);
    }
    
    public FileEntry? FindByComputerIdAndPath(long computerId, string path)
    {
        return _context.FileEntries
            .FirstOrDefault(fe =>
                fe.ComputerId == computerId &&
                fe.Path == path);
    }
    
    public List<FileEntry> FindAllByPath(string path)
    {
        return _context.FileEntries
            .Where(fe => fe.Path == path)
            .ToList();
    }

    public List<FileEntry> FindAll()
    {
        return _context.FileEntries.ToList();
    }

    public FileEntry Update(long id, FileEntry fileEntry)
    {
        var existing = _context.FileEntries.FirstOrDefault(fe => fe.Id == id);

        if (existing == null)
            return null!;
        
        existing.Path = fileEntry.Path;
        existing.Name = fileEntry.Name;
        existing.Extension = fileEntry.Extension;
        existing.SizeBytes = fileEntry.SizeBytes;
        existing.Sha256 = fileEntry.Sha256;
        existing.LastWriteTimeUtc = fileEntry.LastWriteTimeUtc;
        existing.LastSeenUtc = fileEntry.LastSeenUtc;
        existing.CreatedTimeUtc = fileEntry.CreatedTimeUtc;
        existing.LastAccessTimeUtc = fileEntry.LastAccessTimeUtc;
        existing.IsReadOnly = fileEntry.IsReadOnly;
        existing.Attributes = fileEntry.Attributes;

        _context.SaveChanges();

        return existing;
    }

    public void Delete(long id)
    {
        var entity = _context.FileEntries.FirstOrDefault(fe => fe.Id == id);

        if (entity == null)
            return;

        _context.FileEntries.Remove(entity);
        _context.SaveChanges();
    }
}