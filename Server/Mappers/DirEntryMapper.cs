using Contracts.Dtos.Request;
using Server.Dtos.Response;
using Server.Models;

namespace Server.Mappers;

public static class DirEntryMapper
{
    public static DirEntry ToEntity(DirEntryRequestDto dto)
    {
        return new DirEntry
        {
            Path = dto.Path,
            FileCount = dto.FileCount,
            SubdirectoryCount = dto.SubdirectoryCount,
            SizeBytes = dto.SizeBytes,
            CreatedAtUtc = dto.CreatedAtUtc,
            LastSeenAtUtc = dto.LastSeenAtUtc
        };
    }

    public static DirEntryResponseDto ToResponseDto(DirEntry entity)
    {
        return new DirEntryResponseDto
        {
            Id = entity.Id,
            Path = entity.Path,
            FileCount = entity.FileCount,
            SubdirectoryCount = entity.SubdirectoryCount,
            SizeBytes = entity.SizeBytes,
            CreatedAtUtc = entity.CreatedAtUtc,
            LastSeenAtUtc = entity.LastSeenAtUtc,
            SnapshotId = entity.SnapshotId
        };
    }
}