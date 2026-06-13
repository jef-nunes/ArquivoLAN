using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Persistence;
using Server.Services.Interfaces;

namespace Server.Services;

public class ComputerService : IComputerService
{
    private readonly ArquivoLanDbContext _context;

    public ComputerService(ArquivoLanDbContext context)
    {
        _context = context;
    }

    // ==========================================
    // CREATE
    // ==========================================
    public Computer Create(Computer computer)
    {
        computer.FirstSeenUtc = DateTime.UtcNow;
        computer.LastSeenUtc = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(computer.MacAddress))
            computer.MacAddress = NormalizeMac(computer.MacAddress);

        _context.Computers.Add(computer);
        _context.SaveChanges();

        return computer;
    }

    // ==========================================
    // FIND BY ID 
    // ==========================================
    public Computer? FindById(long id)
    {
        return _context.Computers
            .FirstOrDefault(c => c.Id == id);
    }
    
    // ==========================================
    // FIND BY AGENT ID
    // ==========================================
    public Computer? FindByAgentId(string agentId)
    {
        return _context.Computers
            .FirstOrDefault(c =>
                c.OperatingSystem == agentId);
    }

    // ==========================================
    // FIND BY MAC 
    // ==========================================
    public Computer? FindByMacAddress(string macAddress)
    {
        var normalizedMac = NormalizeMac(macAddress);

        return _context.Computers
            .FirstOrDefault(c =>
                c.MacAddress != null &&
                c.MacAddress == normalizedMac);
    }
    
    // ==========================================
    // FIND BY OS 
    // ==========================================
    public Computer? FindByOperatingSystem(string operatingSystem)
    {
        return _context.Computers
            .FirstOrDefault(c =>
                c.OperatingSystem == operatingSystem);
    }
    
    // ==========================================
    // FIND BY Hostname 
    // ==========================================
    public Computer? FindByHostname(string hostname)
    {

        return _context.Computers
            .FirstOrDefault(c =>
               c.Hostname == hostname);
    }

    // ==========================================
    // FIND ALL (leve - sem snapshots)
    // ==========================================
    public List<Computer> FindAll()
    {
        return _context.Computers
            .ToList();
    }

    // ==========================================
    // FIND ALL DETAILED (com snapshots)
    // ==========================================
    public List<Computer> FindAllWithSnapshots()
    {
        return _context.Computers
            .Include(c => c.Snapshots)
            .ToList();
    }

    // ==========================================
    // UPDATE (dados do device apenas)
    // ==========================================
    public Computer? Update(long id, Computer updatedComputer)
    {
        var existing = _context.Computers
            .FirstOrDefault(c => c.Id == id);

        if (existing == null)
            return null;

        // OBS: AgentId não é atualizado
        existing.Hostname = updatedComputer.Hostname;
        existing.Fqdn = updatedComputer.Fqdn;
        existing.DomainName = updatedComputer.DomainName;
        existing.Ipv4Address = updatedComputer.Ipv4Address;
        existing.MacAddress = string.IsNullOrWhiteSpace(updatedComputer.MacAddress)
            ? existing.MacAddress
            : NormalizeMac(updatedComputer.MacAddress);

        existing.OperatingSystem = updatedComputer.OperatingSystem;
        existing.AgentVersion = updatedComputer.AgentVersion;

        existing.LastSeenUtc = DateTime.UtcNow;

        _context.SaveChanges();

        return existing;
    }

    // ==========================================
    // DELETE
    // ==========================================
    public void Delete(long id)
    {
        var entity = _context.Computers
            .FirstOrDefault(c => c.Id == id);

        if (entity == null)
            return;

        _context.Computers.Remove(entity);
        _context.SaveChanges();
    }

    // ==========================================
    // HELPERS
    // ==========================================
    private static string NormalizeMac(string mac)
    {
        return mac
            .Replace(":", "")
            .Replace("-", "")
            .ToUpperInvariant();
    }
}