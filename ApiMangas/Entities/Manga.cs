namespace ApiMangas.Entities;

public class Manga : Entity
{
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public string? Autor { get; set; }
    public string? Editora { get; set; }
    public int Paginas { get; set; }
    public DateTime Publicacao { get; set; }
    public string? Formato { get; set; }
    public string? Cor { get; set; }
    public string? Origem { get; set; }
    public string? Imagem { get; set; } 
    public decimal Preco { get; set; }
    public int Estoque { get; set; }

    //chave estrangeira
    public int CategoriaId { get; set; }
    //relacionamento 
    public Categoria? Categoria { get; set; }
}
