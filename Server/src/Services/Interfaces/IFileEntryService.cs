using Server.Models;

namespace Server.Services.Interfaces;

public interface IFileEntryService : ICrudService<FileEntry>
{
    FileEntry Create(FileEntry fileEntry);
    List<FileEntry>? FindAllByPath(string path);
}