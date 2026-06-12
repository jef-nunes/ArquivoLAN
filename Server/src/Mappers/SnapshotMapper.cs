using Server.Dtos.Request;
using Server.Dtos.Response;
using Server.Models;

namespace Server.Mappers;

public static class SnapshotMapper
{
    public static Snapshot ToEntity(SnapshotRequestDto dto)
    {
        return new Snapshot
        {
            StartedAtUtc = dto.StartedAtUtc,
            FinishedAtUtc = dto.FinishedAtUtc,
            TargetPath = dto.TargetPath
        };
    }

    public static SnapshotResponseDto ToResponseDto(Snapshot entity)
    {
        return new SnapshotResponseDto
        {
            Id = entity.Id,
            StartedAtUtc = entity.StartedAtUtc,
            FinishedAtUtc = entity.FinishedAtUtc,
            TargetPath = entity.TargetPath,
            FileEntryIdList = entity.FileEntries.Select(f => f.Id).ToList(),
            DirEntryId = entity.DirEntry.Id
        };
    }
}