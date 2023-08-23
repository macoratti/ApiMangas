using ApiMangas.ApiPaginacao;
using ApiMangas.DTOs;
using ApiMangas.Entities;
using ApiMangas.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMangas.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
public class MangasController : ControllerBase
{
    private readonly IMangaRepository _mangaRepository;
    private readonly IMapper _mapper;

    public MangasController(IMangaRepository mangaRepository,
        IMapper mapper)
    {
        _mangaRepository = mangaRepository;
        _mapper = mapper;
    }

    [HttpGet("paginacao")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<MangaDTO>>> GetMangasPaginacao([FromQuery] 
                                                    Paginacao paginacao)
    {
        var mangasPaginados = _mangaRepository.GetMangasQueryable();

        if (mangasPaginados is null)
        {
            return NotFound("Mangás não existem");
        }

        double quantidadeRegistrosTotal = await mangasPaginados.CountAsync();
        double totalPaginas = Math.Ceiling(quantidadeRegistrosTotal / paginacao.QuantidadePorPagina);

        var result = await mangasPaginados.Paginar(paginacao).ToListAsync();
        var mangasDto = _mapper.Map<IEnumerable<MangaDTO>>(result);

        var response = new MangaPaginacaoReponseDTO
        {
            Mangas = mangasDto.ToList(),
            TotalPaginas = (int)totalPaginas
        };

        return Ok(response);
    }

    [HttpGet]
    // Atributos de ação que fornecem informações sobre os possíveis códigos de status HTTP
    // que podem ser retornados pelo endpoint da Web API.
    // Esses atributos indicam os códigos de status de resposta esperados para esse endpoint
    // específico e ajudam a documentar e definir a semântica da API
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<MangaDTO>>> GetAll()
    {
        var mangas = await _mangaRepository.GetAllAsync();
        if (mangas is null)
        {
            return NotFound("Mangás não encontrados");
        }

        var mangasDto = _mapper.Map<IEnumerable<MangaDTO>>(mangas);
        return Ok(mangasDto);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var manga = await _mangaRepository.GetByIdAsync(id);

        if (manga is null) return NotFound($"Manga com {id} não encontrado");

        return Ok(_mapper.Map<MangaDTO>(manga));
    }

    [HttpGet]
    [Route("get-mangas-by-category/{categoryId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMangasByCategory(int categoryId)
    {
        var mangas = await _mangaRepository.GetMangasPorCategoriaAsync(categoryId);

        if (!mangas.Any()) return NotFound();

        return Ok(_mapper.Map<IEnumerable<MangaDTO>>(mangas));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add(MangaDTO mangaDto)
    {
        if (!ModelState.IsValid) return BadRequest();

        var manga = _mapper.Map<Manga>(mangaDto);
        await _mangaRepository.AddAsync(manga);

        return Ok(_mapper.Map<MangaDTO>(manga));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, MangaDTO mangaDto)
    {
        if (id != mangaDto.Id) return BadRequest();

        if (!ModelState.IsValid) return BadRequest();

        await _mangaRepository.UpdateAsync(_mapper.Map<Manga>(mangaDto));

        return Ok(mangaDto);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove(int id)
    {
        var manga = await _mangaRepository.GetByIdAsync(id);
        if (manga is null) return NotFound();
        await _mangaRepository.RemoveAsync(manga.Id);
        return Ok();
    }

    [HttpGet]
    [Route("search/{mangaTitulo}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<MangaDTO>>> Search(string mangaTitulo)
    {
           var mangas = await _mangaRepository.SearchAsync(m => m.Titulo.Contains(mangaTitulo));
           
           if (mangas is null)
                 return NotFound("Nenhum mangá foi encontrado");   

           return Ok(_mapper.Map<IEnumerable<MangaDTO>>(mangas));        
    }

    [HttpGet]
    [Route("search-manga-with-category/{criterio}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<MangaCategoriaDTO>>> SearchMangaWithCategory(string criterio)
    {
        var mangas = _mapper.Map<List<Manga>>(await _mangaRepository.LocalizaMangaComCategoriaAsync(criterio));

        if (!mangas.Any())
            return NotFound("Nenhum mangá foi encontrado");

        return Ok(_mapper.Map<IEnumerable<MangaCategoriaDTO>>(mangas));
    }
}
