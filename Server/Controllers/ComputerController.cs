using Microsoft.AspNetCore.Mvc;
using Server.Dtos.Request;
using Server.Dtos.Response;
using Server.Mappers;
using Server.Models;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("api/computers")]
public class ComputerController : ControllerBase
{
    private readonly ICrudService<Computer> _computerService;

    public ComputerController(ICrudService<Computer> computerService)
    {
        _computerService = computerService;
    }

    // Obter todos os computers
    [HttpGet]
    public IActionResult Get()
    {
        var computers = _computerService.FindAll();

        var result = new List<ComputerResponseDto>();

        foreach (var computer in computers)
        {
            result.Add(ComputerMapper.ToResponseDto(computer));
        }

        return Ok(result);
    }

    // Obter computer por id
    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        var computer = _computerService.FindById(id);

        if (computer == null)
            return NotFound();

        return Ok(ComputerMapper.ToResponseDto(computer));
    }

    // Criar computer
    [HttpPost]
    public IActionResult Post([FromBody] ComputerRequestDto dto)
    {
        var entity = ComputerMapper.ToEntity(dto);

        var created = _computerService.Create(entity);

        if (created == null)
            return BadRequest();

        var response = ComputerMapper.ToResponseDto(created);

        return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
    }

    // Atualizar computer
    [HttpPut("{id}")]
    public IActionResult Put(long id, [FromBody] ComputerRequestDto dto)
    {
        var entity = ComputerMapper.ToEntity(dto);

        var updated = _computerService.Update(id, entity);

        if (updated == null)
            return BadRequest();

        return Ok(ComputerMapper.ToResponseDto(updated));
    }

    // Deletar computer
    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        _computerService.Delete(id);
        return NoContent();
    }

    // ======================================
    // Criar novo relacionamento entre Computer e FileEntry
    // ======================================
    [HttpPost("{computerId}/file-entries/{fileEntryId}/bind")]
    public IActionResult BindFileEntry(long computerId, long fileEntryId)
    {
        var result = (_computerService as ComputerService)
            ?.BindFileEntry(computerId, fileEntryId);

        if (result == null)
            return BadRequest();

        return Ok(ComputerMapper.ToResponseDto(result));
    }

    // ======================================
    // Remover relacionamento entre Computer e FileEntry
    // ======================================
    [HttpDelete("{computerId}/file-entries/{fileEntryId}/bind")]
    public IActionResult UnbindFileEntry(long computerId, long fileEntryId)
    {
        var result = (_computerService as ComputerService)
            ?.UnbindFileEntry(computerId, fileEntryId);

        if (result == null)
            return BadRequest();

        return Ok(ComputerMapper.ToResponseDto(result));
    }
}