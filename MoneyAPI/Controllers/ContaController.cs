using Microsoft.AspNetCore.Mvc;
using MoneyAPI.Data;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Services.Interfaces;

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (UsuarioId == null)
                return Unauthorized();

            ResponseDto response = await _contaService.DeleteAsync(id, UsuarioId.Value);

            return DefaultResponse(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (UsuarioId == null)
                return Unauthorized();

            IEnumerable<ContaDto> contas = await _contaService.GetContasAsync(UsuarioId.Value);

            return Ok(contas);
        }

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