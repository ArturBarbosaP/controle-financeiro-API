using AutoMapper;
using MoneyAPI.Helpers;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Usuario;
using MoneyAPI.Models.Entities;
using MoneyAPI.Repositories.Interfaces;
using MoneyAPI.Services.Interfaces;

namespace MoneyAPI.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly ICategoriaRepository _categoryRepository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(IUsuarioRepository repository, ICategoriaRepository categoriaRepository, IAuthService authService, IMapper mapper, ILogger<UsuarioService> logger)
        {
            _repository = repository;
            _categoryRepository = categoriaRepository;
            _authService = authService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseDto> CreateAsync(RequestAddUsuarioDto usuarioDto)
        {
            ResponseDto response = new();

            try
            {
                Usuario usuarioInsert = _mapper.Map<Usuario>(usuarioDto);

                _repository.Add(usuarioInsert);

                if (!await _repository.SaveChanges())
                    throw new Exception("Não foi possível criar no banco!");

                AddCategoriasPadrao(usuarioInsert.Id);

                if (!await _categoryRepository.SaveChanges())
                    throw new Exception("Não foi possível criar as categorias padrões no banco!");

                response.Sucesso = true;
                response.Entidade = _mapper.Map<ResponseUsuarioDto>(usuarioInsert);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no método {Method} | DTO: {@Entidade}", nameof(this.CreateAsync), usuarioDto);
                response.Sucesso = false;
                response.Erro = ex.Message + "\n" + ex.InnerException;
                response.StatusCode = 500;
            }

            return response;
        }

        public async Task<ResponseDto> UpdateAsync(int id, RequestUpdateUsuarioDto usuarioDto)
        {
            ResponseDto response = new();

            try
            {
                Usuario usuario = await _repository.GetUsuarioById(id) ?? throw new NullReferenceException("O usuário não existe!");

                Usuario usuarioUpdate = _mapper.Map(usuarioDto, usuario);
                _repository.Update(usuarioUpdate);

                if (!await _repository.SaveChanges())
                    throw new Exception("Não foi possível atualizar no banco!");

                response.Sucesso = true;
                response.Entidade = _mapper.Map<ResponseUsuarioDto>(usuarioUpdate);
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "Erro de NullReferenceException no método {Method} | ID: {ID} | DTO: {@Entidade}", nameof(this.UpdateAsync), id, usuarioDto);
                response.Sucesso = false;
                response.Erro = ex.Message;
                response.StatusCode = 404;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no método {Method} | ID: {ID} | DTO: {@Entidade}", nameof(this.UpdateAsync), id, usuarioDto);
                response.Sucesso = false;
                response.Erro = ex.Message + "\n" + ex.InnerException;
                response.StatusCode = 500;
            }

            return response;
        }

        public async Task<ResponseDto> UpdatePasswordAsync(int id, RequestPasswordUpdateUsuarioDto passwordDto)
        {
            ResponseDto response = new();

            try
            {
                Usuario usuario = await _repository.GetUsuarioById(id) ?? throw new NullReferenceException("O usuário não existe!");

                if (PasswordHelper.VerifyPassword(passwordDto.SenhaAtual, usuario.Senha))
                {
                    Usuario usuarioUpdate = _mapper.Map(passwordDto, usuario);
                    _repository.Update(usuarioUpdate);

                    if (!await _repository.SaveChanges())
                        throw new Exception("Não foi possível atualizar no banco!");

                    response.Sucesso = true;
                }
                else
                {
                    response.Sucesso = false;
                    response.Erro = "A senha digitada não corresponde com a senha atual!";
                    response.StatusCode = 400;
                }
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "Erro de NullReferenceException no método {Method} | ID: {ID} | DTO: {@Entidade}", nameof(this.UpdatePasswordAsync), id, passwordDto);
                response.Sucesso = false;
                response.Erro = ex.Message;
                response.StatusCode = 404;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no método {Method} | ID: {ID} | DTO: {@Entidade}", nameof(this.UpdatePasswordAsync), id, passwordDto);
                response.Sucesso = false;
                response.Erro = ex.Message + "\n" + ex.InnerException;
                response.StatusCode = 500;
            }

            return response;
        }

        public async Task<ResponseDto> DeleteAsync(int id, string token)
        {
            ResponseDto response = new();

            try
            {
                Usuario usuario = await _repository.GetUsuarioById(id) ?? throw new NullReferenceException("O usuário não existe!");

                ResponseDto responseLogout = await _authService.LogoutAsync(token);

                if (!responseLogout.Sucesso)
                {
                    response.Sucesso = false;
                    response.Erro = responseLogout.Erro;
                    response.StatusCode = responseLogout.StatusCode;
                }
                else
                {
                    await DeleteCategoriasPadrao(usuario.Id);

                    if (!await _categoryRepository.SaveChanges())
                        throw new Exception("Não foi possível excluir as categorias padrões no banco!");

                    _repository.Delete(usuario);

                    if (!await _repository.SaveChanges())
                        throw new Exception("Não foi possível excluir no banco!");

                    response.Sucesso = true;
                }
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "Erro de NullReferenceException no método {Method} | ID: {ID}", nameof(this.DeleteAsync), id);
                response.Sucesso = false;
                response.Erro = ex.Message;
                response.StatusCode = 404;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no método {Method} | ID: {ID}", nameof(this.DeleteAsync), id);
                response.Sucesso = false;
                response.Erro = ex.Message + "\n" + ex.InnerException;
                response.StatusCode = 500;
            }

            return response;
        }

        public async Task<ResponseUsuarioDto?> GetUsuarioByIdAsync(int id)
        {
            return _mapper.Map<ResponseUsuarioDto>(await _repository.GetUsuarioById(id));
        }

        public async Task<ResponseUsuarioDto?> GetUsuarioByNomeUsuarioAsync(string usuario)
        {
            return _mapper.Map<ResponseUsuarioDto>(await _repository.GetUsuarioByNomeUsuario(usuario));
        }

        public async Task<IEnumerable<ResponseUsuarioDto>> GetUsuariosAsync()
        {
            return _mapper.Map<IEnumerable<ResponseUsuarioDto>>(await _repository.GetUsuarios());
        }

        #region Auxiliares

        private void AddCategoriasPadrao(int usuarioId)
        {
            foreach (Categoria cat in Utils.categoriasPadrões)
            {
                _categoryRepository.Add(new Categoria
                {
                    Nome = cat.Nome,
                    Tipo = cat.Tipo,
                    Cor = cat.Cor,
                    Padrao = cat.Padrao,
                    UsuarioId = usuarioId
                });
            }
        }

        private async Task DeleteCategoriasPadrao(int usuarioId)
        {
            foreach (Categoria categoria in await _categoryRepository.GetCategoriasPadroes(usuarioId))
            {
                _categoryRepository.Delete(categoria);
            }
        }

        #endregion
    }
}