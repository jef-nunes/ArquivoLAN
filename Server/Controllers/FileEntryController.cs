using Microsoft.AspNetCore.Mvc;
using Server.Dtos.Request;
using Server.Dtos.Response;
using Server.Mappers;
using Server.Models;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("api/file-entries")]
public class FileEntryController : ControllerBase
{
    private readonly ICrudService<FileEntry> _fileEntryService;

    public FileEntryController(ICrudService<FileEntry> fileEntryService)
    {
        _fileEntryService = fileEntryService;
    }

    // Obter todas as file entries
    [HttpGet]
    public IActionResult Get()
    {
        List<FileEntry> fileEntries = _fileEntryService.FindAll();
        List<FileEntryResponseDto> fileEntriesResponseDtosList = new List<FileEntryResponseDto>();
        foreach (var fileEntry in fileEntries)
        {
            fileEntriesResponseDtosList.Add(FileEntryMapper.ToResponseDto(fileEntry));
        }
        return Ok(fileEntriesResponseDtosList);
    }
    
    // Obter uma file entry específica
    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        var fileEntry = _fileEntryService.FindById(id);
        if (fileEntry == null)
        {
            return NotFound();
        }
        FileEntryResponseDto fileEntryResponseDto = FileEntryMapper.ToResponseDto(fileEntry);
        return Ok(fileEntryResponseDto);
    }
    
    // Criar uma file entry
    [HttpPost]
    public IActionResult Post([FromBody] FileEntryRequestDto fileEntryRequestDto)
    {
        FileEntry fileEntry = FileEntryMapper.ToEntity(fileEntryRequestDto);
        var createdFileEntry = _fileEntryService.Create(fileEntry);
        if (createdFileEntry == null)
        {
            return BadRequest();
        }
        FileEntryResponseDto fileEntryResponseDto = FileEntryMapper.ToResponseDto(createdFileEntry);
        return CreatedAtAction(nameof(Get), new { id = fileEntryResponseDto.Id }, fileEntryResponseDto);
    }
    
    // Atualizar uma file entry
    [HttpPut("{id}")]
    public IActionResult Put(long id, [FromBody] FileEntryRequestDto fileEntryRequestDto)
    {
        FileEntry fileEntry = FileEntryMapper.ToEntity(fileEntryRequestDto);
        var updatedFileEntry = _fileEntryService.Update(id, fileEntry);
        if (updatedFileEntry == null)
        {
            return BadRequest();
        }
        FileEntryResponseDto fileEntryResponseDto = FileEntryMapper.ToResponseDto(updatedFileEntry);
        return Ok(fileEntryResponseDto);
    }
    
    // Apagar uma file entry
    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        _fileEntryService.Delete(id);
        return NoContent();
    }
}