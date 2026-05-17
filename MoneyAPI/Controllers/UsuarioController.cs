using Microsoft.AspNetCore.Mvc;
using MoneyAPI.Data;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Usuario;
using MoneyAPI.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace MoneyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : BaseController
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service, Session session) : base(session)
        {
            _service = service;
        }

        [SwaggerOperation(Summary = "Criar usuário", Description = "Cria um novo usuário")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddUsuarioDto usuarioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseDto response = await _service.CreateAsync(usuarioDto);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Atualizar usuário", Description = "Atualiza o usuário logado (sem atualização de senha)")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUsuarioDto usuarioDto)
        {
            if (UsuarioId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseDto response = await _service.UpdateAsync(UsuarioId.Value, usuarioDto);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Deletar usuário", Description = "Deleta o usuário logado")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            if (UsuarioId == null)
                return Unauthorized();

            ResponseDto response = await _service.DeleteAsync(UsuarioId.Value);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Listar usuários", Description = "Retorna todos os usuários do sistena (apenas admin)")]
        [ProducesResponseType(typeof(IEnumerable<ReadUsuarioDto>), 200)]
        [ProducesResponseType(401)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (UsuarioId == null)
                return Unauthorized();

            if (!IsAdmin)
                return Unauthorized();

            IEnumerable<ReadUsuarioDto> usuarios = await _service.GetUsuariosAsync();

            return Ok(usuarios);
        }

        [SwaggerOperation(Summary = "Buscar usuário por ID", Description = "Retorna o usuário logado ou qualquer usuário pelo ID caso seja admin")]
        [ProducesResponseType(typeof(ReadUsuarioDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpGet("[action]/{id?}")]
        public async Task<IActionResult> GetById(int? id = null)
        {
            if (UsuarioId == null)
                return Unauthorized();

            ReadUsuarioDto usuario;

            if (IsAdmin && id != null)
                usuario = await _service.GetUsuarioByIdAsync((int)id);
            else
                usuario = await _service.GetUsuarioByIdAsync(UsuarioId.Value);

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [SwaggerOperation(Summary = "Buscar usuário por Usuário", Description = "Retorna o usuário pelo username (apenas admin)")]
        [ProducesResponseType(typeof(ReadUsuarioDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpGet("{nomeUsuario}")]
        public async Task<IActionResult> GetByUsuario(string nomeUsuario)
        {
            if (UsuarioId == null)
                return Unauthorized();

            if (!IsAdmin)
                return Unauthorized();

            ReadUsuarioDto usuario = await _service.GetUsuarioByNomeUsuarioAsync(nomeUsuario);

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }
    }
}