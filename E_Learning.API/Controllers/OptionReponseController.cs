using AutoMapper;
using E_learning.Interfaces;
using E_Learning.Domain.Entities;
using E_learning.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OptionReponseController : ControllerBase
{
    private readonly IOptionReponseRepository _optionReponseRepository;
    private readonly IMapper _mapper;

    public OptionReponseController(IOptionReponseRepository optionReponseRepository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(optionReponseRepository);
        ArgumentNullException.ThrowIfNull(mapper);
        _optionReponseRepository = optionReponseRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retourne toutes les options de r�ponse.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var options = await _optionReponseRepository.GetAllAsync(cancellationToken);
        var optionDtos = _mapper.Map<IReadOnlyList<OptionResponseDTO>>(options);
        return Ok(optionDtos);
    }

    /// <summary>
    /// Retourne une option de r�ponse par identifiant.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return BadRequest("Id invalide.");
        }

        var option = await _optionReponseRepository.GetByIdAsync(id, cancellationToken);
        if (option is null)
        {
            return NotFound();
        }

        var optionDto = _mapper.Map<OptionResponseDTO>(option);
        return Ok(optionDto);
    }

    /// <summary>
    /// Cr�e une option de r�ponse.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] OptionResponseDTO optionDto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(optionDto);

        if (string.IsNullOrWhiteSpace(optionDto.Texte))
        {
            return BadRequest("Le texte est obligatoire.");
        }

        var option = _mapper.Map<OptionReponse>(optionDto);
        var createdOption = await _optionReponseRepository.CreateAsync(option, cancellationToken);
        var createdOptionDto = _mapper.Map<OptionResponseDTO>(createdOption);
        return Created($"/api/optionreponse/{createdOptionDto.Id}", createdOptionDto);
    }

    /// <summary>
    /// Met � jour une option de r�ponse.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] OptionResponseDTO optionDto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(optionDto);

        if (id <= 0)
        {
            return BadRequest("Id invalide.");
        }

        if (id != optionDto.Id)
        {
            return BadRequest("L'id de route et l'id du payload doivent correspondre.");
        }

        var option = _mapper.Map<OptionReponse>(optionDto);
        var updated = await _optionReponseRepository.UpdateAsync(option, cancellationToken);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Supprime une option de r�ponse.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return BadRequest("Id invalide.");
        }

        var deleted = await _optionReponseRepository.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Retourne les options de r�ponse pour une question.
    /// </summary>
    [HttpGet("question/{questionId:int}")]
    public async Task<IActionResult> GetByQuestionIdAsync(int questionId, CancellationToken cancellationToken)
    {
        if (questionId <= 0)
        {
            return BadRequest("Id de question invalide.");
        }

        var options = await _optionReponseRepository.GetByQuestionIdAsync(questionId, cancellationToken);
        var optionDtos = _mapper.Map<IReadOnlyList<OptionResponseDTO>>(options);
        return Ok(optionDtos);
    }
}
