using Server.Models;

namespace Server.Services;

public interface IComputerService : ICrudService<Computer>
{
    Computer? FindByMacAddress(string macAddress);
}