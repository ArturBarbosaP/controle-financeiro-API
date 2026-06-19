using AutoMapper;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Conta;
using MoneyAPI.Models.Entities;
using MoneyAPI.Repositories.Interfaces;
using MoneyAPI.Services.Interfaces;

namespace MoneyAPI.Services
{
    public class ContaService : IContaService
    {
        private readonly IContaRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ContaService> _logger;

        public ContaService(IContaRepository repository, IMapper mapper, ILogger<ContaService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseDto> CreateAsync(RequestContaDto contaDto, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                if (await _repository.GetContaByNome(contaDto.Nome, usuarioId) != null) //bloquear se ja existir conta com o mesmo nome
                {
                    response.Sucesso = false;
                    response.Erro = "Já existe uma conta com esse nome!";
                    response.StatusCode = 400;
                    return response;
                }

                Conta contaInsert = _mapper.Map<Conta>(contaDto);
                contaInsert.UsuarioId = usuarioId;

                _repository.Add(contaInsert);

                if (!await _repository.SaveChanges())
                    throw new Exception("Não foi possível criar no banco!");

                response.Sucesso = true;
                response.Entidade = _mapper.Map<ResponseContaDto>(contaInsert);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no método {Method} | DTO: {@Entidade} | UsuarioId: {UsuarioId}", nameof(this.CreateAsync), contaDto, usuarioId);
                response.Sucesso = false;
                response.Erro = ex.Message + "\n" + ex.InnerException;
                response.StatusCode = 500;
            }

            return response;
        }

        public async Task<ResponseDto> UpdateAsync(int id, RequestContaDto contaDto, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                Conta conta = await _repository.GetContaById(id, usuarioId) ?? throw new NullReferenceException("A conta não existe!");

                if (contaDto.Nome != conta.Nome && await _repository.GetContaByNome(contaDto.Nome, usuarioId, id) != null)
                {   //bloquear se ja existir conta com o mesmo nome, caso o nome seja alterado
                    response.Sucesso = false;
                    response.Erro = "Já existe uma conta com o novo nome!";
                    response.StatusCode = 400;
                    return response;
                }

                Conta contaUpdate = _mapper.Map(contaDto, conta);
                _repository.Update(contaUpdate);

                if (!await _repository.SaveChanges())
                    throw new Exception("Não foi possível atualizar no banco!");

                response.Sucesso = true;
                response.Entidade = _mapper.Map<ResponseContaDto>(contaUpdate);
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "Erro de NullReferenceException no método {Method} | ID: {ID} | DTO: {@Entidade} | UsuarioId: {UsuarioId}", nameof(this.UpdateAsync), id, contaDto, usuarioId);
                response.Sucesso = false;
                response.Erro = ex.Message;
                response.StatusCode = 404;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no método {Method} | ID: {ID} | DTO: {@Entidade} | UsuarioId: {UsuarioId}", nameof(this.UpdateAsync), id, contaDto, usuarioId);
                response.Sucesso = false;
                response.Erro = ex.Message + "\n" + ex.InnerException;
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
                _logger.LogError(ex, "Erro de NullReferenceException no método {Method} | ID: {ID} | UsuarioId: {UsuarioId}", nameof(this.DeleteAsync), id, usuarioId);
                response.Sucesso = false;
                response.Erro = ex.Message;
                response.StatusCode = 404;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no método {Method} | ID: {ID} | UsuarioId: {UsuarioId}", nameof(this.DeleteAsync), id, usuarioId);
                response.Sucesso = false;
                response.Erro = ex.Message + "\n" + ex.InnerException;
                response.StatusCode = 500;
            }

            return response;
        }

        public async Task<ResponseContaDto?> GetContaByIdAsync(int id, int usuarioId)
        {
            return _mapper.Map<ResponseContaDto>(await _repository.GetContaById(id, usuarioId));
        }

        public async Task<IEnumerable<ResponseContaDto>> GetContasAsync(int usuarioId)
        {
            return _mapper.Map<IEnumerable<ResponseContaDto>>(await _repository.GetContas(usuarioId));
        }
    }
}