using ApiMangas.Context;
using ApiMangas.Entities;
using ApiMangas.Repositories.Interfaces;

namespace ApiMangas.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context) { }
}
