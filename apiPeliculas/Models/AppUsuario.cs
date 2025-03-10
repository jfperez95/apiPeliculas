using Microsoft.AspNetCore.Identity;

namespace apiPeliculas.Models
{
    public class AppUsuario : IdentityUser
    {
        public string Nombre { get; set; }
    }
}
