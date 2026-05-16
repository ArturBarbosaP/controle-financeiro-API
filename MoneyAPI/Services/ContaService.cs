using AutoMapper;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.Entities;
using MoneyAPI.Repositories.Interfaces;
using MoneyAPI.Services.Interfaces;

namespace MoneyAPI.Services
{
    public class ContaService : IContaService
    {
        private readonly IContaRepository _repository;
        private readonly IMapper _mapper;

        public ContaService(IContaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateAsync(ContaDto contaDto, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                Conta contaInsert = _mapper.Map<Conta>(contaDto);
                contaInsert.UsuarioId = usuarioId;

                _repository.Add(contaInsert);

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

        public async Task<ResponseDto> UpdateAsync(int id, ContaDto contaDto, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                Conta conta = await _repository.GetContaById(id, usuarioId) ?? throw new NullReferenceException("A conta não existe!");

                Conta contaUpdate = _mapper.Map(contaDto, conta);
                _repository.Update(contaUpdate);

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
                Conta conta = await _repository.GetContaById(id, usuarioId) ?? throw new NullReferenceException("A conta não existe!");

                _repository.Delete(conta);

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

        public async Task<ContaDto?> GetContaByIdAsync(int id, int usuarioId)
        {
            return _mapper.Map<ContaDto>(await _repository.GetContaById(id, usuarioId));
        }

        public async Task<IEnumerable<ContaDto>> GetContasAsync(int usuarioId)
        {
            return _mapper.Map<IEnumerable<ContaDto>>(await _repository.GetContas(usuarioId));
        }
    }
}