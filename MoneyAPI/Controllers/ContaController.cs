using Microsoft.AspNetCore.Mvc;
using MoneyAPI.Data;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace MoneyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaController : BaseController
    {
        private readonly IContaService _contaService;

        public ContaController (IContaService contaService, Session session) : base(session)
        {
            _contaService = contaService;
        }

        [SwaggerOperation(Summary = "Criar conta", Description = "Cria uma nova conta para o usuário autenticado")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContaDto contaDto)
        {
            if (UsuarioId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseDto response = await _contaService.CreateAsync(contaDto, UsuarioId.Value);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Atualizar conta", Description = "Atualiza uma conta passando seu ID e as novas informações")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ContaDto contaDto)
        {
            if (UsuarioId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseDto response = await _contaService.UpdateAsync(id, contaDto, UsuarioId.Value);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Deletar conta", Description = "Deleta uma conta passando seu ID")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (UsuarioId == null)
                return Unauthorized();

            ResponseDto response = await _contaService.DeleteAsync(id, UsuarioId.Value);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Listar contas", Description = "Retorna todas as contas para o usuário autenticado")]
        [ProducesResponseType(typeof(IEnumerable<ContaDto>), 200)]
        [ProducesResponseType(401)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (UsuarioId == null)
                return Unauthorized();

            IEnumerable<ContaDto> contas = await _contaService.GetContasAsync(UsuarioId.Value);

            return Ok(contas);
        }

        [SwaggerOperation(Summary = "Buscar conta", Description = "Retorna a conta com o ID informado")]
        [ProducesResponseType(typeof(IEnumerable<ContaDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (UsuarioId == null)
                return Unauthorized();

            ContaDto conta = await _contaService.GetContaByIdAsync(id, UsuarioId.Value);

            if (conta == null)
                return NotFound();

            return Ok(conta);
        }
    }
}