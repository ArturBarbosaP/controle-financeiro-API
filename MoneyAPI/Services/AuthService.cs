using MoneyAPI.Data;
using MoneyAPI.Helpers;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Auth;
using MoneyAPI.Models.Entities;
using MoneyAPI.Repositories.Interfaces;
using MoneyAPI.Services.Interfaces;

namespace MoneyAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _repository;
        private readonly Session _session;

        public AuthService(IUsuarioRepository repository, Session session)
        {
            _repository = repository;
            _session = session;
        }

        public async Task<ResponseDto> LoginAsync(RequestLoginDto loginDTO)
        {
            ResponseDto response = new();

            try
            {
                Usuario usuario = await _repository.GetUsuarioByNomeUsuario(loginDTO.Usuario);

                if (usuario == null || !PasswordHelper.VerifyPassword(loginDTO.Senha, usuario.Senha))
                {
                    response.Sucesso = false;
                    response.Erro = "Usuário ou senha inválidos!";
                    response.StatusCode = 401;
                }
                else
                {
                    var token = _session.CriarSessao(usuario.Id);
                    response.Sucesso = true;
                    response.Entidade = new ResponseLoginDTO
                    {
                        UsuarioId = usuario.Id,
                        Nome = usuario.Nome,
                        Token = token
                    };
                }
            }
            catch (Exception ex)
            {
                response.Sucesso = false;
                response.Erro = ex.Message + "\n" + ex.InnerException;
                response.StatusCode = 500;
            }

            return response;
        }

        public async Task<ResponseDto> LogoutAsync(string token)
        {
            ResponseDto response = new();

            try
            {
                _session.RemoverSessao(token);
                response.Sucesso = true;
            }
            catch (Exception ex)
            {
                response.Sucesso = false;
                response.Erro = ex.Message + "\n" + ex.InnerException;
                response.StatusCode = 500;
            }

            return response;
        }

        public async Task<ResponseDto> VerifyAsync(string token)
        {
            ResponseDto response = new();

            try
            {
                var usuarioId = _session.ObterUsuarioId(token);

                if (usuarioId == null)
                {
                    response.Sucesso = false;
                    response.Erro = "Sessão inválida ou expirada!";
                    response.StatusCode = 401;
                }
                else
                {
                    response.Sucesso = true;
                }
            }
            catch (Exception ex)
            {
                response.Sucesso = false;
                response.Erro = ex.Message + "\n" + ex.InnerException;
                response.StatusCode = 500;
            }

            return response;
        }
    }
}