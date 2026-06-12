using Server.Models;

namespace Server.Services.Interfaces;

public interface IDirEntryService : ICrudService<DirEntry>
{
    DirEntry? FindBySnapshotId(long snapshotId);
    DirEntry? FindByPath(string path);
}