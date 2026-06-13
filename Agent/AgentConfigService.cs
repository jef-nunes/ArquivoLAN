using System.Reflection;
using System.Text.Json;

namespace Agent;

public static class AgentConfigService
{
    // Caminho do arquivo de configuração local do Agent.
    // Sempre relativo ao diretório de execução do binário.
    private static readonly string FilePath =
        Path.Combine(AppContext.BaseDirectory, "agent-properties.json");

    public static AgentProperties GetAgentProperties()
    {
        EnsureJsonFileExists();

        // Carregar configuração atual
        AgentProperties currentAgentProperties = LoadFromJson();

        // Garantir que todas as propriedades foram carregadas
        
        //___________________ Propriedade AgentId _________________________
        string agentId;
        if (string.IsNullOrWhiteSpace(currentAgentProperties.AgentId))
        {

            // Tenta recuperar do backup (variável de ambiente)
            if (AgentEnvironmentService.IsSetAgentIdEnv())
            {
                agentId = AgentEnvironmentService.GetAgentId();
            }
            else
            {
                // Não existe nem no JSON nem na ENV
                // Gera um novo AgentId e salva na ENV
                agentId = Guid.NewGuid().ToString();

                AgentEnvironmentService.SetAgentIdEnv(agentId);
            }
        }
        // Já existe
        else
        {
            agentId = currentAgentProperties.AgentId;
        }
        
        // Adicionalmente, se o JSON possui AgentId mas a ENV não,
        if (!AgentEnvironmentService.IsSetAgentIdEnv())
        {
            AgentEnvironmentService.SetAgentIdEnv(agentId);
        }
        
        //______________________________________________________________
        
        
        

        //___________________ Propriedade AgentVersion _________________________
        string agentVersion;
        if (string.IsNullOrWhiteSpace(currentAgentProperties.AgentVersion))
        {

            agentVersion =
                Assembly.GetExecutingAssembly()
                    .GetName()
                    .Version?
                    .ToString()
                ?? "unknown";
        }
        // Já existe
        else
        {
            agentVersion = currentAgentProperties.AgentVersion;
        }
        //________________________________________________________________
            
        // Objeto AgentProperties com todas as propriedades validadas
            var validAgentProperties = new AgentProperties
            {
                AgentId = agentId,
                AgentVersion = agentVersion
            };

            // Persistir no JSON 
            SaveIntoJson(validAgentProperties);

            return validAgentProperties;
    }

    private static void EnsureJsonFileExists()
    {
        if (File.Exists(FilePath))
        {
            return;
        }

        var agentProperties = new AgentProperties();

        SaveIntoJson(agentProperties);
    }

    private static void SaveIntoJson(
        AgentProperties agentProperties)
    {
        var json = JsonSerializer.Serialize(
            agentProperties,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        File.WriteAllText(FilePath, json);
    }

    private static AgentProperties LoadFromJson()
    {
        var json = File.ReadAllText(FilePath);

        var agentProperties =
            JsonSerializer.Deserialize<AgentProperties>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        if (agentProperties is null)
        {
            throw new InvalidOperationException(
                "Failed to deserialize agent-properties.json.");
        }

        return agentProperties;
    }
}