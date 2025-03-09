﻿using apiPeliculas.Data;
using apiPeliculas.Models;
using apiPeliculas.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace apiPeliculas.Repositorio
{
    public class PeliculaRepositorio : IPeliculaRepositorio
    {
        private readonly ApplicationDbContext _bd;

        public PeliculaRepositorio(ApplicationDbContext bd)
        {
            _bd = bd;
        }

        public bool ActualizarPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            var PeliculaExistente = _bd.Peliculas.Find(pelicula.Id);
            if (PeliculaExistente != null)
            {
                _bd.Entry(PeliculaExistente).CurrentValues.SetValues(pelicula);
            }
            else
            {
                _bd.Peliculas.Update(pelicula);
            }
            return Guardar();
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _bd.Peliculas.Remove(pelicula);
            return Guardar();
        }

        public IEnumerable<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _bd.Peliculas;
            if(!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(e => e.Nombre.Contains(nombre) || e.Descripcion.Contains(nombre));

            }
            return query.ToList();
        }

        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _bd.Peliculas.Add(pelicula);
            return Guardar();
        }

        public bool ExistePelicula(int id)
        {
            return _bd.Peliculas.Any(c => c.Id == id);
        }

        public bool ExistePelicula(string nombre)
        {
            bool valor = _bd.Peliculas.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public Pelicula GetPelicula(int peliculaId)
        {
            return _bd.Peliculas.FirstOrDefault(c => c.Id == peliculaId);
        }

        public ICollection<Pelicula> GetPeliculas()
        {
            return _bd.Peliculas.OrderBy(c => c.Nombre).ToList();
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int catId)
        {
            return _bd.Peliculas.Include(c => c.Categoria).Where(c => c.categoriaId == catId).ToList();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}
