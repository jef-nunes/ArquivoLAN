using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Persistence;

public class ArquivoLanDbContext : DbContext
{
    public ArquivoLanDbContext(DbContextOptions<ArquivoLanDbContext> options) : base(options)
    { 
    }
    
    public DbSet<FileEntry> FileEntries { get; set; }
    public DbSet<Computer> Computers { get; set; }
}