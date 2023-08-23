using ApiMangas.Entities;

namespace ApiMangas.Repositories.Interfaces;

public interface IMangaRepository : IRepository<Manga>
{
    Task<IEnumerable<Manga>>
        GetMangasPorCategoriaAsync(int categoriaId);

    Task<IEnumerable<Manga>>
        LocalizaMangaComCategoriaAsync(string criterio);

    IQueryable<Manga> GetMangasQueryable();   
}
