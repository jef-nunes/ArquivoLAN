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
            AgentVersion = dto.AgentVersion,
            FirstSeenUtc = dto.FirstSeenUtc,
            LastSeenUtc = dto.LastSeenUtc
        };
    }

    public static ComputerResponseDto ToResponseDto(Computer entity)
    {
        List<long> fileEntriesIdList = new List<long>();
        foreach (var fileEntry in entity.FileEntries)
        {
            fileEntriesIdList.Add(fileEntry.Id);
        }
        return new ComputerResponseDto
        {
            Id = entity.Id,
            Hostname = entity.Hostname,
            Fqdn = entity.Fqdn,
            DomainName = entity.DomainName,
            Ipv4Address = entity.Ipv4Address,
            MacAddress = entity.MacAddress,
            OperatingSystem = entity.OperatingSystem,
            AgentVersion = entity.AgentVersion,
            FirstSeenUtc = entity.FirstSeenUtc,
            LastSeenUtc = entity.LastSeenUtc,
            FileEntriesIdList = fileEntriesIdList
        };
    }
}