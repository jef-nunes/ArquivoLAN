using System.Text;
using System.Text.Json;
using Contracts.Dtos.Request;
using DefaultNamespace;

namespace Agent;

// Classe responsável por coletar os dados no dispositivo e enviar ao servidor
public class Program
{
    private static readonly HttpClient HttpClient = new();   
    
    private static async Task Main(string[] args)
    {
        // Carregar propriedades do agente
        Console.WriteLine("Loading agent properties");
        var agentProperties = AgentProperties.Load();

        // Verificar se o ArquivoLAN Server está no ar
        bool serverIsUp = await CheckServerAvailability(agentProperties);
        if (!serverIsUp)
        {
            Console.WriteLine("Server is down");
            Environment.ExitCode = 1;
            return;
        }
        
        // Realizar nova coleta
        GatheringRequestDto result = GatherInfo(agentProperties);
        
        // Enviar ao servidor resultado da coleta
        await PostGatheringResult(agentProperties, result);
    }
    
    // Nova coleta de dados
    private static GatheringRequestDto GatherInfo(AgentProperties agentProperties)
    {
        Console.WriteLine($"Gathering information on target path: {agentProperties.TargetPath}");
        // Criar o objeto de informações do computador
        ComputerRequestDto computer = new ComputerRequestDto
        {
            AgentId = agentProperties.AgentId,
            Hostname = Environment.MachineName,
            DomainName = "Unknown",
            Ipv4Address = Helpers.GetLocalIPv4(),
            AgentVersion = agentProperties.AgentVersion,
            Fqdn = "Unknown",
            MacAddress = Helpers.GetMacAddress(),
            OperatingSystem = Environment.OSVersion.ToString()
        };
        
        // Criar o objeto snapshot, contem dados da coleta atual
        Console.WriteLine("Create snapshot object");
        SnapshotRequestDto snapshot = new SnapshotRequestDto();
        snapshot.StartedAtUtc = DateTime.UtcNow;
        
        // Varrendo o diretório alvo
        Console.WriteLine("Gathering dirs and files information");
        // Salvar os paths dos diretórios e arquivos encontrados
        List<string> foundDirsPathList = GetFoundDirsPathList(agentProperties.TargetPath);
        List<string> foundPathList = GetFoundFilesPathList(agentProperties.TargetPath);
        
        // Criar o restante dos objetos
        Console.WriteLine("Formatting data");
        // Lista com diversos relatórios de diretórios
        List<DirEntryRequestDto> dirEntries = BuildDirEntries(foundDirsPathList);
        // Lista com diversos relatórios de arquivos
        List<FileEntryRequestDto> fileEntries = BuildFileEntries(foundPathList);
        
        // Salvar o datetime do fim da coleta no objeto snapshot
        snapshot.FinishedAtUtc = DateTime.UtcNow;
        
        // Retornar o DTO com o resultado da coleta
        return new GatheringRequestDto
        {
            Computer = computer,
            Snapshot = snapshot,
            DirEntries = dirEntries,
            FileEntries = fileEntries
        };
    }

    // Resultados da varredura do diretório alvo, retornando uma lista de paths de diretórios encontrados
    private static List<string> GetFoundDirsPathList(string targetPath)
    {
        List<string> foundDirsPathList = new();
        if (!Directory.Exists(targetPath))
            throw new DirectoryNotFoundException();
        try
        {
            // raiz
            foundDirsPathList.Add(targetPath);

            // varre diretórios recursivamente
            foreach (var dir in Directory.EnumerateDirectories(targetPath, "*", SearchOption.AllDirectories))
            {
                foundDirsPathList.Add(dir);
            }
        }
        catch (UnauthorizedAccessException)
        {
            // ignora diretórios sem permissão
        }
        catch (PathTooLongException)
        {
            // ignora paths inválidos
        }
        
        return foundDirsPathList;
    }
    
    // Resultados da varredura do diretório alvo, retornando uma lista de paths de arquivos encontrados
    private static List<string> GetFoundFilesPathList(string targetPath)
    {
        List<string> foundFilesPathList = new();
        
            if (!Directory.Exists(targetPath))
                throw new DirectoryNotFoundException();

            try
            {

                // varre arquivos recursivamente
                foreach (var file in Directory.EnumerateFiles(targetPath, "*", SearchOption.AllDirectories))
                {
                    foundFilesPathList.Add(file);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // ignora diretórios sem permissão
            }
            catch (PathTooLongException)
            {
                // ignora paths inválidos
            }

            return foundFilesPathList;

    } 
    
    // Construir a lista de relatórios de diretórios
    private static List<DirEntryRequestDto> BuildDirEntries(List<string> dirsPathList)
    {
        List<DirEntryRequestDto> returnList = new();

        foreach (var dirPath in dirsPathList)
        {
            var dirInfo = new DirectoryInfo(dirPath);
            DirEntryRequestDto dirEntry = new DirEntryRequestDto()
            {
                CreatedAtUtc = dirInfo.CreationTimeUtc,
                LastSeenAtUtc = DateTime.UtcNow,
                FileCount =  dirInfo.GetFiles().Length,
                Path = dirPath,
                SizeBytes =  Helpers.GetDirectorySize(dirPath),
                SubdirectoryCount = Helpers.GetSubdirectoriesCount(dirPath),
            };
            returnList.Add(dirEntry);
        }
        
        return returnList;
    }
    
    // Construir a lista de relatórios de arquivos
    private static List<FileEntryRequestDto> BuildFileEntries(List<string>  filesPathList)
    {
        List<FileEntryRequestDto> returnList = new();
        foreach (var filePath in filesPathList)
        {
            var  fileInfo = new FileInfo(filePath);
            FileEntryRequestDto fileEntry = new FileEntryRequestDto()
            {
                Attributes = fileInfo.Attributes.ToString(),
                CreatedTimeUtc =  fileInfo.CreationTimeUtc,
                Extension = fileInfo.Extension,
                IsReadOnly =  fileInfo.IsReadOnly,
                LastAccessTimeUtc =  fileInfo.LastAccessTimeUtc,
                LastSeenUtc = DateTime.UtcNow,
                LastWriteTimeUtc = fileInfo.LastWriteTimeUtc,
                Name = fileInfo.Name,
                Path = filePath,
                Sha256 = Helpers.GetSha256(filePath)
            };
                returnList.Add(fileEntry);
        }
        return returnList;
    }
    
    // Verificar se o servidor está no ar
    private static async Task<bool> CheckServerAvailability(AgentProperties agentProperties)
    {
        var url = agentProperties.ServerUrl+agentProperties.HealthEndpoint;
        var response = await HttpClient.GetAsync(url);
        return response.IsSuccessStatusCode;
    }
    
    // Postar o resultado no ArquivoLAN Server
    private static async Task PostGatheringResult(
        AgentProperties agentProperties,
        GatheringRequestDto payload)
    {
        Console.WriteLine("Sending gathering results to ArquivoLAN Server");
        var url = agentProperties.ServerUrl+agentProperties.GatheringEndpoint;

        var json = JsonSerializer.Serialize(payload);

        var content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json"
        );

        var response = await HttpClient.PostAsync(url, content);

        var responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine(response.StatusCode);
        Console.WriteLine(responseBody);
    }
}