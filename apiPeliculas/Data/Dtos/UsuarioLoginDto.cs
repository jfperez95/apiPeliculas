using System.ComponentModel.DataAnnotations;

namespace apiPeliculas.Data.Dtos
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El password de usuario es requerido")]
        public string Password { get; set; }
    }
}
