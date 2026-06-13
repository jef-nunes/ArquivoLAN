namespace Contracts;

public record ServerConnectionContract(
    string ServerUrl,
    string GatheringEndpoint,
    string HealthEndpoint);