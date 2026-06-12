using Server.Dtos.Request;
using Server.Dtos.Response;
using Server.Models;

namespace Server.Mappers;

public static class ComputerMapper
{
    public static Computer ToEntity(ComputerRequestDto dto)
    {
        return new Computer
        {
            Hostname = dto.Hostname,
            Fqdn = dto.Fqdn,
            DomainName = dto.DomainName,
            Ipv4Address = dto.Ipv4Address,
            MacAddress = dto.MacAddress,
            OperatingSystem = dto.OperatingSystem,
            AgentId = dto.AgentId,
            AgentVersion = dto.AgentVersion
        };
    }

    public static ComputerResponseDto ToResponseDto(Computer entity)
    {
        return new ComputerResponseDto
        {
            Id = entity.Id,
            Hostname = entity.Hostname,
            Fqdn = entity.Fqdn,
            DomainName = entity.DomainName,
            Ipv4Address = entity.Ipv4Address,
            MacAddress = entity.MacAddress,
            OperatingSystem = entity.OperatingSystem,
            AgentId = entity.AgentId,
            AgentVersion = entity.AgentVersion,
            FirstSeenUtc = entity.FirstSeenUtc,
            LastSeenUtc = entity.LastSeenUtc,
            SnapshotIdList = entity.Snapshots
                .Select(s => s.Id)
                .ToList() ?? new List<long>()
        };
    }
}