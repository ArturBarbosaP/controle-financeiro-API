using MoneyAPI.Data;
using Serilog.Context;

namespace MoneyAPI.Helpers
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly Session _session;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, Session session)
        {
            _next = next;
            _logger = logger;
            _session = session;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var token = context.Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var usuarioId = token != null ? _session.ObterUsuarioId(token)?.ToString() : "Não autenticado";

                using(LogContext.PushProperty("UsuarioId", usuarioId))
                using(LogContext.PushProperty("IP", context.Connection.RemoteIpAddress))
                {
                    _logger.LogError(ex, "Exception não tratada | {Method} {Path} | UsuarioId: {UsuarioId}", context.Request.Method, context.Request.Path, usuarioId);
                }
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { mensagem = "Erro interno do servidor" });
            }
        }
    }
}