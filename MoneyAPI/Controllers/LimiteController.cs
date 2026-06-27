using Microsoft.AspNetCore.Mvc;
using MoneyAPI.Data;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Limite;
using MoneyAPI.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace MoneyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LimiteController : BaseController
    {
        private readonly ILimiteService _service;

        public LimiteController(ILimiteService service, Session session) : base(session)
        {
            _service = service;
        }

        [SwaggerOperation(Summary = "Criar limite", Description = "Cria um novo limite para o usuário autenticado")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RequestLimiteDto limiteDto)
        {
            if (UsuarioId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseDto response = await _service.CreateAsync(limiteDto, UsuarioId.Value);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Atualizar limite", Description = "Atualiza um limite passando seu ID e as novas informações")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RequestLimiteDto limiteDto)
        {
            if (UsuarioId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseDto response = await _service.UpdateAsync(id, limiteDto, UsuarioId.Value);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Deletar limite", Description = "Deleta um limite passando seu ID")]
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

        [SwaggerOperation(Summary = "Listar limites", Description = "Retorna todas os limites para o usuário autenticado")]
        [ProducesResponseType(typeof(IEnumerable<ResponseLimiteDto>), 200)]
        [ProducesResponseType(401)]
        [HttpGet("mes/{data}")]
        public async Task<IActionResult> GetAll(DateOnly data)
        {
            if (UsuarioId == null)
                return Unauthorized();

            IEnumerable<ResponseLimiteDto> limites = await _service.GetLimitesAsync(UsuarioId.Value, data.Month, data.Year);

            return Ok(limites);
        }

        [SwaggerOperation(Summary = "Buscar limite", Description = "Retorna o limite com o ID informado")]
        [ProducesResponseType(typeof(IEnumerable<ResponseLimiteDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpGet("mes/{data}/{id}")]
        public async Task<IActionResult> GetById(DateOnly data, int id)
        {
            if (UsuarioId == null)
                return Unauthorized();

            ResponseLimiteDto? limite = await _service.GetLimiteByIdAsync(id, data.Month, data.Year, UsuarioId.Value);

            if (limite == null)
                return NotFound();

            return Ok(limite);
        }
    }
}