using Microsoft.AspNetCore.Mvc;
using MoneyAPI.Data;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Categoria;
using MoneyAPI.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace MoneyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : BaseController
    {
        private readonly ICategoriaService _service;

        public CategoriaController(ICategoriaService service, Session session) : base(session)
        {
            _service = service;
        }

        [SwaggerOperation(Summary = "Criar categoria", Description = "Cria uma nova categoria para o usuário autenticado")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RequestCategoriaDto categoriaDto)
        {
            if (UsuarioId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseDto response = await _service.CreateAsync(categoriaDto, UsuarioId.Value);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Atualizar categoria", Description = "Atualiza uma categoria passando seu ID e as novas informações")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RequestCategoriaDto categoriaDto)
        {
            if (UsuarioId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseDto response = await _service.UpdateAsync(id, categoriaDto, UsuarioId.Value);

            return DefaultResponse(response);
        }

        [SwaggerOperation(Summary = "Deletar categoria", Description = "Deleta uma categoria passando seu ID")]
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

        [SwaggerOperation(Summary = "Listar categorias", Description = "Retorna todas as categorias para o usuário autenticado")]
        [ProducesResponseType(typeof(IEnumerable<ResponseCategoriaDto>), 200)]
        [ProducesResponseType(401)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (UsuarioId == null)
                return Unauthorized();

            IEnumerable<ResponseCategoriaDto> categoria = await _service.GetCategoriasAsync(UsuarioId.Value);

            return Ok(categoria);
        }

        [SwaggerOperation(Summary = "Buscar categoria", Description = "Retorna a categoria com o ID informado")]
        [ProducesResponseType(typeof(IEnumerable<ResponseCategoriaDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (UsuarioId == null)
                return Unauthorized();

            ResponseCategoriaDto categoria = await _service.GetCategoriaByIdAsync(id, UsuarioId.Value);

            if (categoria == null)
                return NotFound();

            return Ok(categoria);
        }
    }
}