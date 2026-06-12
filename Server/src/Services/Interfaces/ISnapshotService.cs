using Server.Models;

namespace Server.Services.Interfaces;

public interface ISnapshotService : ICrudService<Snapshot>
{
    // Overload
    Snapshot Create(Snapshot snapshot, long ComputerId);
    List<Snapshot>? FindAllByTimeRange(DateTime from, DateTime to);
}