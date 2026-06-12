using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Persistence;

namespace Server.Services;

public class ComputerService : IComputerService
{
    private readonly ArquivoLanDbContext _context;

    public ComputerService(ArquivoLanDbContext context)
    {
        _context = context;
    }

    public Computer Create(Computer computer)
    {
        computer.FirstSeenUtc = DateTime.UtcNow;
        computer.LastSeenUtc = DateTime.UtcNow;

        _context.Computers.Add(computer);
        _context.SaveChanges();

        return computer;
    }

    public Computer? FindById(long id)
    {
        return _context.Computers
            .Include(c => c.FileEntries)
            .FirstOrDefault(c => c.Id == id);
    }
    
    public Computer? FindByMacAddress(string macAddress)
    {
        return _context.Computers
            .Include(c => c.FileEntries)
            .FirstOrDefault(c => c.MacAddress == macAddress);
    }

    public List<Computer> FindAll()
    {
        return _context.Computers
            .Include(c => c.FileEntries)
            .ToList();
    }

    public Computer Update(long id, Computer computer)
    {
        var existing = _context.Computers
            .Include(c => c.FileEntries)
            .FirstOrDefault(c => c.Id == id);

        if (existing == null)
            return null!;

        existing.Hostname = computer.Hostname;
        existing.Fqdn = computer.Fqdn;
        existing.DomainName = computer.DomainName;
        existing.Ipv4Address = computer.Ipv4Address;
        existing.MacAddress = computer.MacAddress;
        existing.OperatingSystem = computer.OperatingSystem;
        existing.AgentVersion = computer.AgentVersion;

        existing.LastSeenUtc = DateTime.UtcNow;

        _context.SaveChanges();

        return existing;
    }

    public void Delete(long id)
    {
        var entity = _context.Computers
            .Include(c => c.FileEntries)
            .FirstOrDefault(c => c.Id == id);

        if (entity == null)
            return;

        _context.Computers.Remove(entity);
        _context.SaveChanges();
    }
    
    // ================================
    // Relacionar este Computer a um FileEntry
    // ================================
    public Computer? BindFileEntry(long computerId, long fileEntryId)
    {
        var computer = _context.Computers
            .Include(c => c.FileEntries)
            .FirstOrDefault(c => c.Id == computerId);

        var fileEntry = _context.FileEntries
            .FirstOrDefault(f => f.Id == fileEntryId);

        if (computer == null || fileEntry == null)
            return null;

        // garante 1:N (FileEntry só pode pertencer a 1 Computer)
        fileEntry.ComputerId = computer.Id;

        if (!computer.FileEntries.Any(f => f.Id == fileEntryId))
        {
            computer.FileEntries.Add(fileEntry);
        }

        _context.SaveChanges();

        return computer;
    }
    
    
    // ================================
    // Remover relacionamento deste Computer com um FileEntry
    // ================================
    public Computer? UnbindFileEntry(long computerId, long fileEntryId)
    {
        var computer = _context.Computers
            .Include(c => c.FileEntries)
            .FirstOrDefault(c => c.Id == computerId);

        var fileEntry = _context.FileEntries
            .FirstOrDefault(f => f.Id == fileEntryId);

        if (computer == null || fileEntry == null)
            return null;

        // remove relação 1:N
        if (computer.FileEntries.Any(f => f.Id == fileEntryId))
        {
            computer.FileEntries.Remove(fileEntry);
        }

        // quebra relação no lado dependente
        if (fileEntry.ComputerId == computer.Id)
        {
            fileEntry.ComputerId = 0; // ou null se mudar para nullable depois
        }

        _context.SaveChanges();

        return computer;
    }
}