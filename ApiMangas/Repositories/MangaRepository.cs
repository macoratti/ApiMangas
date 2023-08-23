using ApiMangas.Context;
using ApiMangas.Entities;
using ApiMangas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiMangas.Repositories;

public class MangaRepository : Repository<Manga>, IMangaRepository
{
    public MangaRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Manga>> GetMangasPorCategoriaAsync(int categoriaId)
    {
        var mangas = await _db.Mangas.Include(b => b.Categoria)
                           .Where(b => b.CategoriaId == categoriaId).ToListAsync();
        return mangas;
    }

    public IQueryable<Manga> GetMangasQueryable()
    {
        return _db.Mangas.AsQueryable();
    }

    public async Task<IEnumerable<Manga>> LocalizaMangaComCategoriaAsync(string criterio)
    {
        return await _db.Mangas.AsNoTracking()
            .Include(b => b.Categoria)
            .Where(b => b.Titulo.Contains(criterio) ||
                        b.Autor.Contains(criterio) ||
                        b.Descricao.Contains(criterio) ||
                        b.Categoria.Nome.Contains(criterio))
            .ToListAsync();
    }
}