namespace apiPeliculas.Data.Dtos
{
    public class CrearPeliculaDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public string RutaImagen { get; set; }
        public enum CrearTipoClasificacion { Siete, Trece, Dieciseis, Dieziocho }
        public CrearTipoClasificacion Clasificacion { get; set; }

        //Relacion con Categoria
        public int categoriaId { get; set; }
    }
}
