using Microsoft.AspNetCore.Mvc;
using MoneyAPI.Data;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Lancamento;
using MoneyAPI.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace MoneyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LancamentoController : BaseController
    {
        private readonly ILancamentoService _service;

        public LancamentoController(ILancamentoService service, Session session) : base(session)
        {
            _service = service;
        }

        [SwaggerOperation(Summary = "Criar lançamento", Description = "Cria um novo lançamento para o usuário autenticado")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RequestLancamentoDto lancamentoDto)
        {
            if (UsuarioId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseDto response = await _service.CreateAsync(lancamentoDto, UsuarioId.Value);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Deletar lançamento", Description = "Deleta um lançamento passando seu ID")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (UsuarioId == null)
                return Unauthorized();

            ResponseDto response = await _service.DeleteAsync(id, UsuarioId.Value);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Listar lançamentos", Description = "Retorna todos os lançamentos do mês selecionado para o usuário autenticado")]
        [ProducesResponseType(typeof(IEnumerable<ResponseLancamentoDto>), 200)]
        [ProducesResponseType(401)]
        [HttpGet]
        public async Task<IActionResult> GetAll(DateOnly data)
        {
            if (UsuarioId == null)
                return Unauthorized();

            IEnumerable<ResponseLancamentoDto> lancamentos = await _service.GetLancamentosMensalAsync(UsuarioId.Value, data.Month, data.Year);

            return Ok(lancamentos);
        }

        [SwaggerOperation(Summary = "Buscar lançamento", Description = "Retorna o lançamento com o ID informado")]
        [ProducesResponseType(typeof(IEnumerable<ResponseLancamentoDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (UsuarioId == null)
                return Unauthorized();

            ResponseLancamentoDto lancamento = await _service.GetLancamentoByIdAsync(id, 1);

            if (lancamento == null)
                return NotFound();

            return Ok(lancamento);
        }
    }
}