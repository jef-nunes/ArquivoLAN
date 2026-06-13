using Server.Models;

namespace Server.Services.Interfaces;

public interface IComputerService : ICrudService<Computer>
{
    Computer Create(Computer computer);
    Computer? FindByMacAddress(string macAddress);
    Computer? FindByHostname(string hostname);
    Computer? FindByOperatingSystem(string operatingSystem);
    Computer? FindByAgentId(string agentId);
}