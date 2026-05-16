using AutoMapper;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.Entities;
using MoneyAPI.Repositories.Interfaces;
using MoneyAPI.Services.Interfaces;

namespace MoneyAPI.Services
{
    public class LimiteService : ILimiteService
    {
        private readonly ILimiteRepository _repository;
        private readonly IMapper _mapper;

        public LimiteService(ILimiteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateAsync(LimiteDto limiteDto, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                Limite limiteInsert = _mapper.Map<Limite>(limiteDto);

                _repository.Add(limiteInsert);

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

        public async Task<ResponseDto> UpdateAsync(int id, LimiteDto limiteDto, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                Limite limite = await _repository.GetLimiteById(id, usuarioId) ?? throw new NullReferenceException("O limite não existe!");

                Limite limiteUpdate = _mapper.Map(limiteDto, limite);
                _repository.Update(limiteUpdate);

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

        public async Task<ResponseDto> DeleteAsync(int id, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                Limite limite = await _repository.GetLimiteById(id, usuarioId) ?? throw new NullReferenceException("O limite não existe!");

                _repository.Delete(limite);

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

        public async Task<LimiteDto?> GetLimiteByIdAsync(int id, int usuarioId)
        {
            return _mapper.Map<LimiteDto>(await _repository.GetLimiteById(id, usuarioId));
        }

        public async Task<IEnumerable<LimiteDto>> GetLimitesAsync(int usuarioId)
        {
            return _mapper.Map<IEnumerable<LimiteDto>>(await _repository.GetLimites(usuarioId));
        }
    }
}