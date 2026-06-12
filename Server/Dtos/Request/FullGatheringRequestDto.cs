namespace Server.Dtos.Request;

public record FullGatheringRequestDto
{
    public required ComputerRequestDto Computer { get; set; }
    public List<FileEntryRequestDto> FileEntries { get; set; }
        = new();
}