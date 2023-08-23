using ApiMangas.Entities;
using System.Linq.Expressions;

namespace ApiMangas.Repositories.Interfaces;

public interface IRepository<T> : IDisposable where T : Entity
{
    Task AddAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int? id);
    Task UpdateAsync(T entity);
    Task RemoveAsync(int? id);

    //Esse método é projetado para realizar uma busca assíncrona
    //no contexto do EF Core, usando um predicado
    //(predicate) fornecido como argumento. O predicado será aplicado
    //à entidade T, que é o tipo genérico usado no método, para filtrar
    //os resultados da busca.
    Task<IEnumerable<T>>
        SearchAsync(Expression<Func<T, bool>> predicate);
}
