using ApiMangas.Validation;

namespace ApiMangas.Entities;

public sealed class Categoria : Entity
{
    public Categoria()
    { }       
    public string? Nome { get; private set; }
    public string? IconCSS { get; private set; }
    public Categoria(string nome, string iconCss)
    {
        ValidateDomain(nome, iconCss);
    }
    public Categoria(int id, string nome, string iconCss)
    {
        DomainExceptionValidation.When(id < 0, "Id inválido.");
        Id = id;
        ValidateDomain(nome, iconCss);
    }
    public void Update(string nome, string iconCss)
    {
        ValidateDomain(nome, iconCss);
    }
    private void ValidateDomain(string nome, string iconCss)
    {
        DomainExceptionValidation.When(string.IsNullOrEmpty(nome),
            "Nome é obrigatório");
        DomainExceptionValidation.When(nome.Length < 3,
           "Nome inválido");
        Nome = nome;

        DomainExceptionValidation.When(string.IsNullOrEmpty(iconCss),
           "Nome do ícone é obrigatório");
        DomainExceptionValidation.When(iconCss.Length < 3,
           "Nome do ícone inválido");
        IconCSS = iconCss;
    }

    public IEnumerable<Manga>? Mangas { get; set; }
}
