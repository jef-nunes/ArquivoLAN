namespace Agent;

using Contracts.Dtos.Request;

public static class AgentGatheringService
{
    // Nova coleta de dados
    // Retorna um DTO que será utilizado
    // como payload no POST ao servidor
    public static GatheringRequestDto Run(AgentProperties agentProperties, string targetPath)
    {
        Console.WriteLine($"Gathering information on target path: {targetPath}");
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
        List<string> foundDirsPathList = GetFoundDirsPathList(targetPath);
        List<string> foundPathList = GetFoundFilesPathList(targetPath);
        
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
                FileCount = Directory.EnumerateFiles(
                    dirPath,
                    "*",
                    SearchOption.AllDirectories
                ).Count(),
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
}