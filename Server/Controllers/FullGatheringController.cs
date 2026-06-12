using Microsoft.AspNetCore.Mvc;
using Server.Dtos.Request;
using Server.Mappers;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("api/agent/full-gathering")]
public class FullGatheringController : ControllerBase
{
    private readonly IComputerService _computerService;
    private readonly IFileEntryService _fileEntryService;

    public FullGatheringController(
        IComputerService computerService,
        IFileEntryService fileEntryService)
    {
        _computerService = computerService;
        _fileEntryService = fileEntryService;
    }

    // =========================================================
    // Recebe o payload completo do Agent (Computer + FileEntries)
    // =========================================================
    [HttpPost]
    public IActionResult Post([FromBody] FullGatheringRequestDto dto)
    {
        if (dto == null || dto.Computer == null || dto.FileEntries == null)
            return BadRequest("Invalid payload.");

        // =====================================================
        // 1. Criar ou atualizar Computer
        // =====================================================
        var computerEntity = ComputerMapper.ToEntity(dto.Computer);

        var existingComputer = _computerService
            .FindByMacAddress(dto.Computer.MacAddress);

        long computerId;

        if (existingComputer == null)
        {
            var created = _computerService.Create(computerEntity);
            computerId = created.Id;
        }
        else
        {
            _computerService.Update(existingComputer.Id, computerEntity);
            computerId = existingComputer.Id;
        }

        // =====================================================
        // 2. Processar FileEntries (UPSERT por ComputerId + Path)
        // =====================================================
        foreach (var fileDto in dto.FileEntries)
        {
            var entity = FileEntryMapper.ToEntity(fileDto);
            entity.ComputerId = computerId;

            var existingFile = _fileEntryService
                .FindByComputerIdAndPath(computerId, fileDto.Path);

            if (existingFile == null)
            {
                _fileEntryService.Create(entity);
            }
            else
            {
                _fileEntryService.Update(existingFile.Id, entity);
            }
        }

        return Ok();
    }
}