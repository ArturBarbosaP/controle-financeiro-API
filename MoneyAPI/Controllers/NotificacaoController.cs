using Microsoft.AspNetCore.Mvc;
using MoneyAPI.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace MoneyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacaoController : BaseController
    {
        private readonly Notification _notification;

        public NotificacaoController(Session session, Notification notification) : base(session)
        {
            _notification = notification;
        }

        [SwaggerOperation(Summary = "Listar notificações", Description = "Retorna todas as notificações para o usuário autenticado")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [HttpGet]
        public IActionResult GetAll()
        {
            if (UsuarioId == null)
                return Unauthorized();

            return Ok(_notification.GetAll(UsuarioId.Value));
        }

        [SwaggerOperation(Summary = "Deletar notificações", Description = "Deleta todas as notificações para o usuário autenticado")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [HttpDelete]
        public IActionResult Delete()
        {
            if (UsuarioId == null)
                return Unauthorized();

            _notification.DeleteAll(UsuarioId.Value);
            return Ok();
        }
    }
}