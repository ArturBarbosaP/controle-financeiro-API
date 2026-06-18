using AutoMapper;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Limite;
using MoneyAPI.Models.Entities;
using MoneyAPI.Repositories.Interfaces;
using MoneyAPI.Services.Interfaces;

namespace MoneyAPI.Services
{
    public class LimiteService : ILimiteService
    {
        private readonly ILimiteRepository _repository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LimiteService> _logger;

        public LimiteService(ILimiteRepository repository, ICategoriaRepository categoriaRepository, IMapper mapper, ILogger<LimiteService> logger)
        {
            _repository = repository;
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseDto> CreateAsync(RequestLimiteDto limiteDto, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                Categoria categoria = await _categoriaRepository.GetCategoriaById(limiteDto.CategoriaId, usuarioId) ?? throw new NullReferenceException("Categoria não encontrada!");
                Limite limiteInsert = _mapper.Map<Limite>(limiteDto);

                _repository.Add(limiteInsert);

                if (!await _repository.SaveChanges())
                    throw new Exception("Não foi possível criar no banco!");

                response.Sucesso = true;
                response.Entidade = _mapper.Map<ResponseLimiteDto>(limiteInsert);
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "Erro de NullReferenceException no método {Method} | DTO: {@Entidade} | UsuarioId: {UsuarioId}", nameof(this.CreateAsync), limiteDto, usuarioId);
                response.Sucesso = false;
                response.Erro = ex.Message;
                response.StatusCode = 404;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no método {Method} | DTO: {@Entidade} | UsuarioId: {UsuarioId}", nameof(this.CreateAsync), limiteDto, usuarioId);
                response.Sucesso = false;
                response.Erro = ex.Message + "\n" + ex.InnerException;
                response.StatusCode = 500;
            }

            return response;
        }

        public async Task<ResponseDto> UpdateAsync(int id, RequestLimiteDto limiteDto, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                Limite limite = await _repository.GetLimiteById(id, usuarioId) ?? throw new NullReferenceException("O limite não existe!");
                Categoria categoria = await _categoriaRepository.GetCategoriaById(limiteDto.CategoriaId, usuarioId) ?? throw new NullReferenceException("Categoria não encontrada!");

                Limite limiteUpdate = _mapper.Map(limiteDto, limite);
                _repository.Update(limiteUpdate);

                if (!await _repository.SaveChanges())
                    throw new Exception("Não foi possível atualizar no banco!");

                response.Sucesso = true;
                response.Entidade = _mapper.Map<ResponseLimiteDto>(limiteUpdate);
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "Erro de NullReferenceException no método {Method} | ID: {ID} | DTO: {@Entidade} | UsuarioId: {UsuarioId}", nameof(this.UpdateAsync), id, limiteDto, usuarioId);
                response.Sucesso = false;
                response.Erro = ex.Message;
                response.StatusCode = 404;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no método {Method} | ID: {ID} | DTO: {@Entidade} | UsuarioId: {UsuarioId}", nameof(this.UpdateAsync), id, limiteDto, usuarioId);
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
                Limite limite = await _repository.GetLimiteById(id, usuarioId) ?? throw new NullReferenceException("O limite não existe!");

                _repository.Delete(limite);

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

        public async Task<ResponseLimiteDto?> GetLimiteByIdAsync(int id, int usuarioId)
        {
            return _mapper.Map<ResponseLimiteDto>(await _repository.GetLimiteById(id, usuarioId));
        }

        public async Task<IEnumerable<ResponseLimiteDto>> GetLimitesAsync(int usuarioId)
        {
            return _mapper.Map<IEnumerable<ResponseLimiteDto>>(await _repository.GetLimites(usuarioId));
        }
    }
}