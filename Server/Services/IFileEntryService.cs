using Server.Models;

namespace Server.Services;

public interface IFileEntryService : ICrudService<FileEntry>
{
    List<FileEntry>? FindAllByPath(string path);
    FileEntry? FindByComputerIdAndPath(long computerId, string path);
}