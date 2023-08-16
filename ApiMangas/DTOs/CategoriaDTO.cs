using System.ComponentModel.DataAnnotations;

namespace ApiMangas.DTOs;

public class CategoriaDTO
{
    public int Id { get; set; }
 
    [Required(ErrorMessage = "O Nome é requerido")]
    [MinLength(3)]
    [MaxLength(100)]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "O Nome do ícone é requerido")]
    [MinLength(3)]
    [MaxLength(100)]
    public string? IconCSS { get; set; }

}
