using Contracts;

namespace Agent;

public static class ServerConnectionContractFactory
{
    public static ServerConnectionContract Create(string serverUrl)
    {
        if (string.IsNullOrWhiteSpace(serverUrl))
        {
            throw new ArgumentException(
                "Server URL cannot be empty.");
        }

        return new ServerConnectionContract(
            ServerUrl: serverUrl.TrimEnd('/'),
            GatheringEndpoint: "/api/gathering",
            HealthEndpoint: "/api/health");
    }
}