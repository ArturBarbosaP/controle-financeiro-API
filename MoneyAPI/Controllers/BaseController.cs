using Microsoft.AspNetCore.Mvc;
using MoneyAPI.Data;
using MoneyAPI.Models.DTOs;

namespace MoneyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly Session _session;

        protected BaseController(Session session)
        {
            _session = session;
        }

        protected int? UsuarioId
        {
            get
            {
                #if DEBUG
                    return 1;
                #endif

                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
                return token is null ? null : _session.ObterUsuarioId(token);
            }
        }

        protected IActionResult DefaultResponse (ResponseDto response)
        {
            return response.Sucesso ? Ok() : StatusCode(response.StatusCode == 0 ? 500 : response.StatusCode, response.Erro);
        }
    }
}