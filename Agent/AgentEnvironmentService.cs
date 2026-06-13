namespace Agent;

public static class AgentEnvironmentService
{
    private const string EnvServerUrlKey = "ARQUIVOLAN_SERVER_URL";
    private const string EnvAgentIdKey = "ARQUIVOLAN_AGENT_ID";

    public static string GetServerUrl()
    {
        return GetRequiredEnv(EnvServerUrlKey);
    }

    public static bool IsSetAgentIdEnv()
    {
        return IsSetEnv(EnvAgentIdKey);
    }

    public static string GetAgentId()
    {
        return GetRequiredEnv(EnvAgentIdKey);
    }

    public static void SetAgentIdEnv(string agentId)
    {
        SetRequiredEnv(EnvAgentIdKey, agentId);
    }

    private static void SetRequiredEnv(string key, string value)
    {
        Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.User);
    }
    
    private static string GetRequiredEnv(string key)
    {
        var value = Environment.GetEnvironmentVariable(
            key,
            EnvironmentVariableTarget.User);

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException(
                $"Environment variable {key} is required.");
        }

        return value;
    }
    
    private static bool IsSetEnv(string key)
    {
        var value = Environment.GetEnvironmentVariable(
            key,
            EnvironmentVariableTarget.User);

        return !string.IsNullOrWhiteSpace(value);
    }
}