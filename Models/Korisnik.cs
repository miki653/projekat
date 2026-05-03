using System.ComponentModel.DataAnnotations;

namespace KatalogFilmova.Models;

public class Korisnik
{
    [Required(ErrorMessage = "Korisnicko ime je obavezno")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Korisnicko ime mora imati 3 do 30 karaktera")]
    public string username { get; set; } = "";

    [Required(ErrorMessage = "Lozinka je obavezna")]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "Lozinka mora imati bar 4 karaktera")]
    [DataType(DataType.Password)]
    public string password { get; set; } = "";
}
