using apiPeliculas.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace apiPeliculas.Data.Dtos
{
    public class PeliculaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public string RutaImagen { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieziocho }
        public TipoClasificacion Clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }

        //Relacion con Categoria
        public int categoriaId { get; set; }
    }
}
