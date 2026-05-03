using System.ComponentModel.DataAnnotations;

namespace KatalogFilmova.Models;

public class Film
{
    public int id { get; set; }

    [Required(ErrorMessage = "Naziv je obavezan")]
    [StringLength(80, ErrorMessage = "Naziv moze imati najvise 80 karaktera")]
    public string naz { get; set; } = "";

    [Required(ErrorMessage = "Zanr je obavezan")]
    [StringLength(40, ErrorMessage = "Zanr moze imati najvise 40 karaktera")]
    public string zanr { get; set; } = "";

    [Range(1900, 2100, ErrorMessage = "Godina mora biti izmedju 1900 i 2100")]
    public int god { get; set; }

    [Range(1, 10, ErrorMessage = "Ocena mora biti od 1 do 10")]
    public double oc { get; set; }

    [Required(ErrorMessage = "Opis je obavezan")]
    [StringLength(500, ErrorMessage = "Opis moze imati najvise 500 karaktera")]
    public string opis { get; set; } = "";

    [StringLength(250)]
    public string img { get; set; } = "";

    [Required(ErrorMessage = "Tip je obavezan")]
    [RegularExpression("film|serija", ErrorMessage = "Tip mora biti film ili serija")]
    public string tip { get; set; } = "film";
}
