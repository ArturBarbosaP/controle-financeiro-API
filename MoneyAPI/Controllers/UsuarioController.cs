using Microsoft.AspNetCore.Mvc;
using MoneyAPI.Data;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Auth;
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
        private readonly IAuthService _authService;

        public UsuarioController(IUsuarioService service, IAuthService authService, Session session) : base(session)
        {
            _service = service;
            _authService = authService;
        }

        #region Usuarios

        [SwaggerOperation(Summary = "Criar usuário", Description = "Cria um novo usuário")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RequestAddUsuarioDto usuarioDto)
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
        public async Task<IActionResult> Update([FromBody] RequestUpdateUsuarioDto usuarioDto)
        {
            if (UsuarioId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseDto response = await _service.UpdateAsync(UsuarioId.Value, usuarioDto);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Atualizar senha do usuário", Description = "Atualiza a senha do usuário logado")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdatePassword([FromBody] RequestPasswordUpdateUsuarioDto passwordDto)
        {
            if (UsuarioId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseDto response = await _service.UpdatePasswordAsync(UsuarioId.Value, passwordDto);

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

            ResponseDto response = await _service.DeleteAsync(UsuarioId.Value, Token);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Listar usuários", Description = "Retorna todos os usuários do sistena (apenas admin)")]
        [ProducesResponseType(typeof(IEnumerable<ResponseUsuarioDto>), 200)]
        [ProducesResponseType(401)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (UsuarioId == null)
                return Unauthorized();

            if (!IsAdmin)
                return Unauthorized();

            IEnumerable<ResponseUsuarioDto> usuarios = await _service.GetUsuariosAsync();

            return Ok(usuarios);
        }

        [SwaggerOperation(Summary = "Buscar usuário por ID", Description = "Retorna o usuário logado ou qualquer usuário pelo ID caso seja admin")]
        [ProducesResponseType(typeof(ResponseUsuarioDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpGet("{id?}")]
        public async Task<IActionResult> GetById(int? id = null)
        {
            if (UsuarioId == null)
                return Unauthorized();

            ResponseUsuarioDto usuario;

            if (IsAdmin && id != null)
                usuario = await _service.GetUsuarioByIdAsync((int)id);
            else
                usuario = await _service.GetUsuarioByIdAsync(UsuarioId.Value);

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [SwaggerOperation(Summary = "Buscar usuário por Usuário", Description = "Retorna o usuário pelo username (apenas admin)")]
        [ProducesResponseType(typeof(ResponseUsuarioDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpGet("{nomeUsuario}")]
        public async Task<IActionResult> GetByUsuario(string nomeUsuario)
        {
            if (UsuarioId == null)
                return Unauthorized();

            if (!IsAdmin)
                return Unauthorized();

            ResponseUsuarioDto usuario = await _service.GetUsuarioByNomeUsuarioAsync(nomeUsuario);

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        #endregion

        #region Autenticacao

        [SwaggerOperation(Summary = "Fazer login", Description = "Faz login no sistema com Usuário e Senha")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpPost("Auth/[action]")]
        public async Task<IActionResult> Login([FromBody] RequestLoginDto loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseDto response = await _authService.LoginAsync(loginDTO);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Verificar sessão ativa", Description = "Verifica se a sessão está ativa")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpGet("Auth/[action]")]
        public async Task<IActionResult> Verify()
        {
            if (string.IsNullOrEmpty(Token))
                return Unauthorized();

            ResponseDto response = await _authService.VerifyAsync(Token);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Fazer logout", Description = "Faz logout do sistema")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpGet("Auth/[action]")]
        public async Task<IActionResult> Logout()
        {
            if (string.IsNullOrEmpty(Token))
                return Unauthorized();

            ResponseDto response = await _authService.LogoutAsync(Token);

            return DefaultResponse(response);
        }

        #endregion
    }
}