using Microsoft.AspNetCore.Mvc;
using MoneyAPI.Data;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Cartao;
using MoneyAPI.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace MoneyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartaoController : BaseController
    {
        private readonly ICartaoService _service;

        public CartaoController(ICartaoService service, Session session) : base(session)
        {
            _service = service;
        }

        [SwaggerOperation(Summary = "Criar cartão", Description = "Cria um novo cartão para o usuário autenticado")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RequestCartaoDto cartaoDto)
        {
            if (UsuarioId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseDto response = await _service.CreateAsync(cartaoDto, UsuarioId.Value);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Atualizar cartão", Description = "Atualiza um cartão passando seu ID e as novas informações")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RequestCartaoDto cartaoDto)
        {
            if (UsuarioId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseDto response = await _service.UpdateAsync(id, cartaoDto, UsuarioId.Value);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Deletar cartão", Description = "Deleta um cartão passando seu ID")]
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

        [SwaggerOperation(Summary = "Listar cartões", Description = "Retorna todos os cartões para o usuário autenticado")]
        [ProducesResponseType(typeof(IEnumerable<RequestCartaoDto>), 200)]
        [ProducesResponseType(401)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (UsuarioId == null)
                return Unauthorized();

            IEnumerable<ResponseCartaoDto> cartoes = await _service.GetCartoesAsync(UsuarioId.Value);

            return Ok(cartoes);
        }

        [SwaggerOperation(Summary = "Buscar cartão", Description = "Retorna o cartão com o ID informado")]
        [ProducesResponseType(typeof(IEnumerable<RequestCartaoDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (UsuarioId == null)
                return Unauthorized();

            ResponseCartaoDto cartao = await _service.GetCartaoByIdAsync(id, UsuarioId.Value);

            if (cartao == null)
                return NotFound();

            return Ok(cartao);
        }

        [SwaggerOperation(Summary = "Paga a fatura em aberto do cartão", Description = "Paga a fatura em aberto do cartão passando seu ID")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpPut("{id}/[action]")]
        public async Task<IActionResult> PagarFatura(int id, [FromBody] RequestCartaoDto cartaoDto)
        {
            if (UsuarioId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseDto response = await _service.PagarFatura(id, cartaoDto, UsuarioId.Value);

            return DefaultResponse(response);
        }
    }
}