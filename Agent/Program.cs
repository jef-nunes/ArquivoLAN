using System.Text.Json;
using Contracts.Dtos.Request;

namespace Agent;

// Classe responsável por coletar os dados no dispositivo e enviar ao servidor
public class Program
{
    private static async Task RunNoServerMode(string targetPath)
    {
        // Carregar propriedades do agente
        Console.WriteLine("Loading agent properties");
        AgentProperties agentProperties = AgentConfigService.GetAgentProperties();

        // Realizar nova coleta
        GatheringRequestDto gatheringResult = AgentGatheringService.Run(agentProperties, targetPath);
        
        // Serializar para JSON e imprimir o resultado no terminal
        string gatheringResultJson = JsonSerializer.Serialize(
            gatheringResult,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        Console.WriteLine(gatheringResultJson);
    }
    
    private static async Task RunDefaultMode(string targetPath)
    {
        ServerHttpClient serverHttpClient = new();

        // Verificar se o servidor está no ar
        bool serverIsUp = await serverHttpClient.IsServerAvailableAsync();
        if (!serverIsUp)
        {
            Console.WriteLine("Server is down");
            Environment.ExitCode = 1;
            return;
        }

        // Carregar propriedades do agente
        Console.WriteLine("Loading agent properties");
        AgentProperties agentProperties = AgentConfigService.GetAgentProperties();

        // Realizar nova coleta
        GatheringRequestDto gatheringResult = AgentGatheringService.Run(agentProperties, targetPath);

        // Enviar ao servidor resultado da coleta
        await serverHttpClient.PostGatheringAsync(gatheringResult);
    }

    private static async Task Main(string[] args)
    {
        // Lógica de argumentos de execução

        // O argumento "--no-server" indica que o app apenas coleta os dados
        // e imprime no terminal o resultado da coleta. Isto é útil para testes
        // sem interagir com o servidor da aplicação.
        bool noServer = args.Contains(
            "--no-server",
            StringComparer.OrdinalIgnoreCase);

        string[] remainingArgs = args
            .Where(arg => !arg.Equals(
                "--no-server",
                StringComparison.OrdinalIgnoreCase))
            .ToArray();

        // O outro único argumento é o <target-path> o qual indica em qual
        // diretório o Agent realizará a coleta de dados
        if (remainingArgs.Length != 1)
        {
            Console.WriteLine(
                "Usage: ArquivoLAN-Agent.exe <target-path> [--no-server]");

            Environment.ExitCode = 1;
            return;
        }

        string targetPath = remainingArgs[0];

        if (!Directory.Exists(targetPath))
        {
            Console.WriteLine(
                $"Directory not found: {targetPath}");

            Environment.ExitCode = 1;
            return;
        }
        
        // Fluxo sem enviar resultado pro servidor
        if (noServer)
        {
            await RunNoServerMode(targetPath);
        }
        // Fluxo enviando resultado pro servidor
        else
        {
            await RunDefaultMode(targetPath);
        }
    }
}