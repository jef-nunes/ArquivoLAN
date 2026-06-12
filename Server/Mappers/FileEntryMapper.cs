using Server.Dtos.Request;
using Server.Dtos.Response;
using Server.Models;

namespace Server.Mappers;

public static class FileEntryMapper
{
    public static FileEntry ToEntity(FileEntryRequestDto dto)
    {
        return new FileEntry
        {
            Path = dto.Path,
            Name = dto.Name,
            Extension = dto.Extension,
            SizeBytes = dto.SizeBytes,
            Sha256 = dto.Sha256,
            LastWriteTimeUtc = dto.LastWriteTimeUtc,
            LastSeenUtc = dto.LastSeenTimeUtc,
            CreatedTimeUtc = dto.CreatedTimeUtc,
            LastAccessTimeUtc = dto.LastAccessTimeUtc,
            IsReadOnly = dto.IsReadOnly,
            Attributes = dto.Attributes
        };
    }

    public static FileEntryResponseDto ToResponseDto(FileEntry entity)
    {
        return new FileEntryResponseDto
        {
            Id = entity.Id,
            ComputerId = entity.ComputerId,
            Path = entity.Path,
            Name = entity.Name,
            Extension = entity.Extension,
            SizeBytes = entity.SizeBytes,
            Sha256 = entity.Sha256,
            LastWriteTimeUtc = entity.LastWriteTimeUtc,
            LastSeenUtc = entity.LastSeenUtc,
            CreatedTimeUtc = entity.CreatedTimeUtc,
            LastAccessTimeUtc = entity.LastAccessTimeUtc,
            IsReadOnly = entity.IsReadOnly,
            Attributes = entity.Attributes
        };
    }
}