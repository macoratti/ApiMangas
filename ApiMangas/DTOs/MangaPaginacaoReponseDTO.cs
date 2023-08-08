namespace ApiMangas.DTOs;

public class MangaPaginacaoReponseDTO
{
    public List<MangaDTO>? Mangas { get; set; }
    public int TotalPaginas { get; set; }
}
