using Microsoft.AspNetCore.Mvc;
using Contracts.Dtos.Request;
using Server.Mappers;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Controllers;

[ApiController]
[Route("api/gathering")]
public class GatheringController : ControllerBase
{
    private readonly IComputerService _computerService;
    private readonly ISnapshotService _snapshotService;

    public GatheringController(
        IComputerService computerService,
        ISnapshotService snapshotService)
    {
        _computerService = computerService;
        _snapshotService = snapshotService;
    }

    [HttpPost]
    public IActionResult Post(
        [FromBody] GatheringRequestDto requestDto)
    {
        if (requestDto == null)
        {
            return BadRequest();
        }

        // ==========================================
        // Localizar ou criar Computer
        // ==========================================

        Computer? computer =
            _computerService.FindByAgentId(
                requestDto.Computer.AgentId);

        if (computer == null)
        {
            computer = _computerService.Create(
                ComputerMapper.ToEntity(
                    requestDto.Computer));
        }
        else
        {
            computer = _computerService.Update(
                computer.Id,
                ComputerMapper.ToEntity(
                    requestDto.Computer));
        }

        if (computer == null)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError);
        }

        // ==========================================
        // Montar Snapshot completo
        // ==========================================

        Snapshot snapshot =
            SnapshotMapper.ToEntity(
                requestDto.Snapshot);

        foreach (var dirEntryDto in requestDto.DirEntries)
        {
            snapshot.DirEntries.Add(
                DirEntryMapper.ToEntity(
                    dirEntryDto));
        }

        foreach (var fileEntryDto in requestDto.FileEntries)
        {
            snapshot.FileEntries.Add(
                FileEntryMapper.ToEntity(
                    fileEntryDto));
        }

        // ==========================================
        // Persistir Snapshot
        // ==========================================

        Snapshot createdSnapshot =
            _snapshotService.Create(
                snapshot,
                computer.Id);

        if (createdSnapshot == null)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError);
        }

        return Ok(new
        {
            SnapshotId = createdSnapshot.Id
        });
    }
}