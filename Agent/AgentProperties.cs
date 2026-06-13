using System.Reflection;

namespace Agent;

public record AgentProperties
{
    public string AgentId { get; init; } = string.Empty;
    public string AgentVersion { get; init; } = string.Empty;
}