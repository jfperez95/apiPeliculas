using System.Net;
using apiPeliculas.Data.Dtos;
using apiPeliculas.Models;
using apiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apiPeliculas.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usRepo;
        protected RespuestaAPI _respuestaAPI;
        private readonly IMapper _mapper;

        public UsuariosController(IUsuarioRepositorio usRepo, IMapper mapper)
        {
            _usRepo = usRepo;
            _mapper = mapper;
            this._respuestaAPI = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetUsuario()
        {
            var listaCategorias = _usRepo.GetUsuarios();
            var listaUsuarioDto = new List<UsuarioDto>();
            foreach (var lista in listaUsuarioDto)
            {
                listaUsuarioDto.Add(_mapper.Map<UsuarioDto>(lista));
            }
            return Ok(listaUsuarioDto);
        }

        [HttpGet("{usuarioId:int}", Name = "GetUsuario")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetUsuario(int usuarioId)
        {
            var itemUsuario = _usRepo.GetUsuario(usuarioId);

            if (itemUsuario == null)
            {
                return NotFound();
            }

            var itemUsuarioDto = _mapper.Map<CategoriaDto>(itemUsuario);

            return Ok(itemUsuarioDto);
        }

        [HttpPost("registro")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> registro([FromBody] UsuarioRegistroDto usuarioRegistroDto)
        {
            bool validarNombreUsuarioUnico = _usRepo.IsUniqueUser(usuarioRegistroDto.NombreUsuario);
            if(!validarNombreUsuarioUnico)
            {
                _respuestaAPI.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaAPI.IsSucces = false;
                _respuestaAPI.ErrorMessages.Add("El nombre ya existe");
                return BadRequest(_respuestaAPI);
            }

            var usuario = await _usRepo.Registro(usuarioRegistroDto);
            if(usuario == null)
            {
                _respuestaAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.IsSucces = false;
                _respuestaAPI.ErrorMessages.Add("Error al registrar");
                return BadRequest(_respuestaAPI);
            }

            _respuestaAPI.StatusCode = HttpStatusCode.OK;
            _respuestaAPI.IsSucces = true;
            return Ok(_respuestaAPI);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> login([FromBody] UsuarioLoginDto usuarioLoginDto)
        {
            var respuestaLogin = await _usRepo.Login(usuarioLoginDto);
            if (respuestaLogin == null ||  string.IsNullOrEmpty(respuestaLogin.Token))
            {
                _respuestaAPI.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaAPI.IsSucces = false;
                _respuestaAPI.ErrorMessages.Add("El nombre de usuario o password son incorrectos");
                return BadRequest(_respuestaAPI);
            }

            
            _respuestaAPI.StatusCode = HttpStatusCode.OK;
            _respuestaAPI.IsSucces = true;
            _respuestaAPI.Result = respuestaLogin;
            return Ok(_respuestaAPI);
            
        }
    }
}
