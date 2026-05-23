using Microsoft.AspNetCore.Mvc;
using MoneyAPI.Data;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Auth;
using MoneyAPI.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace MoneyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service, Session session) : base(session)
        {
            _service = service;
        }

        [SwaggerOperation(Summary = "Fazer login", Description = "Faz login no sistema com Usuário e Senha")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseDto response = await _service.LoginAsync(loginDTO);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Verificar sessão ativa", Description = "Verifica se a sessão está ativa")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpGet("[action]")]
        public async Task<IActionResult> Verify()
        {
            if (string.IsNullOrEmpty(Token))
                return BadRequest();

            ResponseDto response = await _service.VerifyAsync(Token);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Fazer logout", Description = "Faz logout do sistema")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpGet("[action]")]
        public async Task<IActionResult> Logout()
        {
            if (string.IsNullOrEmpty(Token))
                return BadRequest();

            ResponseDto response = await _service.LogoutAsync(Token);

            return DefaultResponse(response);
        }
    }
}