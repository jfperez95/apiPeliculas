using System.ComponentModel.DataAnnotations;

namespace apiPeliculas.Data.Dtos
{
    public class UsuarioRegistroDto
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El password de usuario es requerido")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
