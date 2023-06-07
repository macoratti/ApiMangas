using ApiMangas.Validation;

namespace ApiMangas.Entities;

public sealed class Categoria : Entity
{
    public string? Nome { get; private set; }
    public Categoria(string nome)
    {
        ValidateDomain(nome);
    }
    public Categoria(int id, string nome)
    {
        DomainExceptionValidation.When(id < 0, "Id inválido.");
        Id = id;
        ValidateDomain(nome);
    }
    public void Update(string nome)
    {
        ValidateDomain(nome);
    }
    private void ValidateDomain(string nome)
    {
        DomainExceptionValidation.When(string.IsNullOrEmpty(nome),
            "Nome é obrigatório");

        DomainExceptionValidation.When(nome.Length < 3,
           "Nome inválido");

        Nome = nome;
    }
    public IEnumerable<Manga>? Mangas { get; set; }
}


