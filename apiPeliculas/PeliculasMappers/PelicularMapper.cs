using apiPeliculas.Data.Dtos;
using apiPeliculas.Models;
using AutoMapper;

namespace apiPeliculas.PeliculasMapper
{
    public class PelicularMapper : Profile
    {
        public PelicularMapper()
        {
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Categoria, CrearCategoriaDto>().ReverseMap();
        }
    }
}
