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

        protected string? Token
        {
            get
            {
                return Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
            }
        }

        protected int? UsuarioId
        {
            get
            {
#if DEBUG
                return 1;
#endif
                return Token is null ? null : _session.ObterUsuarioId(Token);
            }
        }

        protected bool IsAdmin
        {
            get
            {
#if DEBUG
                return true;
#endif
                return Token is not null && _session.UsuarioInAdminList(Token);
            }
        }

        protected IActionResult DefaultResponse (ResponseDto response)
        {
            return response.Sucesso ? (response.Entidade != null ? Ok(response.Entidade) : Ok()) : StatusCode(response.StatusCode == 0 ? 500 : response.StatusCode, response.Erro);
        }
    }
}