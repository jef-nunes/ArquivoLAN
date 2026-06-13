namespace Agent;

public record AgentProperties
{
    public string AgentId { get; init; } = string.Empty;
    public string AgentVersion { get; init; } = string.Empty;
    public string ServerUrl { get; init; } = string.Empty;
    public string GatheringEndpoint { get; init; } = string.Empty;
    public string HealthEndpoint { get; init; } = string.Empty;
    public string TargetPath { get; init; } = string.Empty;

    private const string FilePath = "agent-properties.json";

    public static AgentProperties Load()
    {
        var json = File.ReadAllText(FilePath);

        var config = System.Text.Json.JsonSerializer.Deserialize<AgentProperties>(
            json,
            new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

        if (string.IsNullOrWhiteSpace(config.AgentId))
        {
            config = config with
            {
                AgentId = Guid.NewGuid().ToString()
            };

            Save(config);
        }

        return config;
    }

    private static void Save(AgentProperties config)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(
            config,
            new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });

        File.WriteAllText(FilePath, json);
    }
}