using AutoMapper;
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

        public async Task<ResponseDto> CreateAsync(AddUsuarioDto usuarioDto)
        {
            ResponseDto response = new();

            try
            {
                Usuario usuarioInsert = _mapper.Map<Usuario>(usuarioDto);

                _repository.Add(usuarioInsert);

                if (!await _repository.SaveChanges())
                    throw new Exception("Não foi possível criar no banco!");

                response.Sucesso = true;
            }
            catch (Exception ex)
            {
                response.Sucesso = false;
                response.Erro = ex.Message;
                response.StatusCode = 500;
            }

            return response;
        }

        public async Task<ResponseDto> UpdateAsync(int id, UpdateUsuarioDto usuarioDto)
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
                response.Erro = ex.Message;
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
                response.Erro = ex.Message;
                response.StatusCode = 500;
            }

            return response;
        }

        public async Task<ReadUsuarioDto?> GetUsuarioByIdAsync(int id)
        {
            return _mapper.Map<ReadUsuarioDto>(await _repository.GetUsuarioById(id));
        }

        public async Task<ReadUsuarioDto?> GetUsuarioByNomeUsuarioAsync(string usuario)
        {
            return _mapper.Map<ReadUsuarioDto>(await _repository.GetUsuarioByNomeUsuario(usuario));
        }

        public async Task<IEnumerable<ReadUsuarioDto>> GetUsuariosAsync()
        {
            return _mapper.Map<IEnumerable<ReadUsuarioDto>>(await _repository.GetUsuarios());
        }
    }
}