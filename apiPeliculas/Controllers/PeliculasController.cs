﻿using apiPeliculas.Data.Dtos;
using apiPeliculas.Models;
using apiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apiPeliculas.Controllers
{
    //[Authorize]
    [Route("api/peliculas")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaRepositorio _pelRepo;
        private readonly IMapper _mapper;

        public PeliculasController(IPeliculaRepositorio pelRepo, IMapper mapper)
        {
            _pelRepo = pelRepo;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetPeliculas()
        {
            var listaPeliculas = _pelRepo.GetPeliculas();
            var listaPeliculasDto = new List<PeliculaDto>();
            foreach (var lista in listaPeliculas)
            {
                listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(lista));
            }
            return Ok(listaPeliculasDto);
        }

        [AllowAnonymous]
        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetPelicula(int peliculaId)
        {
            var itemPelicula = _pelRepo.GetPelicula(peliculaId);

            if (itemPelicula == null)
            {
                return NotFound();
            }

            var itemPeliculaDto = _mapper.Map<PeliculaDto>(itemPelicula);

            return Ok(itemPeliculaDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PeliculaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public IActionResult CrearPelicula([FromBody] CrearPeliculaDto crearPeliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (crearPeliculaDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_pelRepo.ExistePelicula(crearPeliculaDto.Nombre))
            {
                ModelState.AddModelError("", $"La pelicula ya existe");
                return StatusCode(404, ModelState);
            }

            var pelicula = _mapper.Map<Pelicula>(crearPeliculaDto);

            if (!_pelRepo.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro{pelicula.Nombre}");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id }, pelicula);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{peliculaId:int}", Name = "ActualizarPatchPelicula")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public IActionResult ActualizarPatchPelicula(int peliculaId, [FromBody] PeliculaDto peliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (peliculaDto == null || peliculaId != peliculaDto.Id)
            {
                return BadRequest(ModelState);
            }

            var peliculaExistente = _pelRepo.GetPelicula(peliculaId);
            if(peliculaExistente == null)
            {
                return NotFound($"No se encontró la pelicula con ID {peliculaId}");
            }

            var pelicula = _mapper.Map<Pelicula>(peliculaDto);

            if (!_pelRepo.ActualizarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando la pelicula {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{peliculaId:int}", Name = "ActualizarPutPelicula")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult ActualizarPutPelicula(int peliculaId, [FromBody] PeliculaDto peliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (peliculaDto == null || peliculaId != peliculaDto.Id)
            {
                return BadRequest(ModelState);
            }

            var peliculaExistente = _pelRepo.GetPelicula(peliculaId);

            if (peliculaExistente == null)
            {
                return NotFound($"No se encontro la categoria con ID {peliculaId}");
            }

            var pelicula = _mapper.Map<Pelicula>(peliculaDto);

            if (!_pelRepo.ActualizarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro{pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult BorrarPelicula(int peliculaId)
        {

            if (!_pelRepo.ExistePelicula(peliculaId))
            {
                return NotFound($"No se encontro la categoria con ID {peliculaId}");
            }

            var pelicula = _pelRepo.GetPelicula(peliculaId);

            if (!_pelRepo.BorrarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal borrando el registro{pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("GetPeliculasEnCategoria/{categoriaId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetPeliculasEnCategoria(int categoriaId)
        {
            var listaPeliculas = _pelRepo.GetPeliculasEnCategoria(categoriaId);
            if(listaPeliculas == null)
            {
                return NotFound();
            }
            var listaPeliculasDto = new List<PeliculaDto>();
            foreach (var pelicula in listaPeliculas)
            {
                listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(pelicula));
            }
            return Ok(listaPeliculasDto);
        }

        [AllowAnonymous]
        [HttpGet("Buscar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Buscar(string nombre)
        {
            try
            {
                var resultado = _pelRepo.BuscarPelicula(nombre);
                if (resultado.Any())
                {
                    return Ok(resultado);
                }

                return NotFound();

            }catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos de la aplicación");
            }
            
        }
    }
}
