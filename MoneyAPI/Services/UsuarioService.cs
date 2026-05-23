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
        private readonly IMapper _mapper;

        public UsuarioService(IUsuarioRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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

                response.Sucesso = true;
                response.Entidade = _mapper.Map<ResponseUsuarioDto>(usuarioInsert);
            }
            catch (Exception ex)
            {
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
                response.Sucesso = false;
                response.Erro = ex.Message;
                response.StatusCode = 404;

            }
            catch (Exception ex)
            {
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
                response.Sucesso = false;
                response.Erro = ex.Message;
                response.StatusCode = 404;

            }
            catch (Exception ex)
            {
                response.Sucesso = false;
                response.Erro = ex.Message + "\n" + ex.InnerException;
                response.StatusCode = 500;
            }

            return response;
        }

        public async Task<ResponseDto> DeleteAsync(int id)
        {
            ResponseDto response = new();

            try
            {
                Usuario usuario = await _repository.GetUsuarioById(id) ?? throw new NullReferenceException("O usuário não existe!");

                _repository.Delete(usuario);

                if (!await _repository.SaveChanges())
                    throw new Exception("Não foi possível excluir no banco!");

                response.Sucesso = true;
            }
            catch (NullReferenceException ex)
            {
                response.Sucesso = false;
                response.Erro = ex.Message;
                response.StatusCode = 404;

            }
            catch (Exception ex)
            {
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
    }
}