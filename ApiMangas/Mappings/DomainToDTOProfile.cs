using AutoMapper;
using ApiMangas.DTOs;
using ApiMangas.Entities;

namespace ApiMangas.Mappings;

public class DomainToDTOProfile : Profile
{
    public DomainToDTOProfile()
    {
        CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        CreateMap<Manga, MangaDTO>().ReverseMap();

        // cria um mapeamento entre a classe Manga e a classe MangaCategoriaDTO.
        // O mapeamento especifica que a propriedade NomeCategoria do DTO será
        // mapeada a partir da propriedade Nome da propriedade Categoria do objeto Manga.
        CreateMap<Manga, MangaCategoriaDTO>()
            .ForMember(dto => dto.NomeCategoria, opt=> opt.MapFrom(src=> src.Categoria.Nome));
    }
}
