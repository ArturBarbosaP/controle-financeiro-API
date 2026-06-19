using AutoMapper;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Categoria;
using MoneyAPI.Models.Entities;
using MoneyAPI.Repositories.Interfaces;
using MoneyAPI.Services.Interfaces;

namespace MoneyAPI.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriaService> _logger;

        public CategoriaService(ICategoriaRepository repository, IMapper mapper, ILogger<CategoriaService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseDto> CreateAsync(RequestCategoriaDto categoriaDto, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                if (await _repository.GetCategoriaByNome(categoriaDto.Nome, categoriaDto.Tipo, usuarioId) != null) //bloquear se ja existir categoria com o mesmo nome
                {
                    response.Sucesso = false;
                    response.Erro = "Já existe uma categoria com esse nome!";
                    response.StatusCode = 400;
                    return response;
                }

                Categoria categoriaInsert = _mapper.Map<Categoria>(categoriaDto);
                categoriaInsert.UsuarioId = usuarioId;

                _repository.Add(categoriaInsert);

                if (!await _repository.SaveChanges())
                    throw new Exception("Não foi possível criar no banco!");

                response.Sucesso = true;
                response.Entidade = _mapper.Map<ResponseCategoriaDto>(categoriaInsert);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no método {Method} | DTO: {@Entidade} | UsuarioId: {UsuarioId}", nameof(this.CreateAsync), categoriaDto, usuarioId);
                response.Sucesso = false;
                response.Erro = ex.Message + "\n" + ex.InnerException;
                response.StatusCode = 500;
            }

            return response;
        }

        public async Task<ResponseDto> UpdateAsync(int id, RequestCategoriaDto categoriaDto, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                Categoria categoria = await _repository.GetCategoriaById(id, usuarioId) ?? throw new NullReferenceException("A categoria não existe!");

                if (categoriaDto.Nome != categoria.Nome && await _repository.GetCategoriaByNome(categoriaDto.Nome, categoriaDto.Tipo, usuarioId, id) != null)
                {   //bloquear se ja existir categoria com o mesmo nome, caso o nome seja alterado
                    response.Sucesso = false;
                    response.Erro = "Já existe uma categoria com o novo nome!";
                    response.StatusCode = 400;
                    return response;
                }

                if (categoria.Padrao)
                {
                    response.Sucesso = false;
                    response.Erro = "Não é possível alterar uma categoria padrão do sistema!";
                    response.StatusCode = 401;
                    return response;
                }

                Categoria categoriaUpdate = _mapper.Map(categoriaDto, categoria);
                _repository.Update(categoriaUpdate);

                if (!await _repository.SaveChanges())
                    throw new Exception("Não foi possível atualizar no banco!");

                response.Sucesso = true;
                response.Entidade = _mapper.Map<ResponseCategoriaDto>(categoriaUpdate);
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "Erro de NullReferenceException no método {Method} | ID: {ID} | DTO: {@Entidade} | UsuarioId: {UsuarioId}", nameof(this.UpdateAsync), id, categoriaDto, usuarioId);
                response.Sucesso = false;
                response.Erro = ex.Message;
                response.StatusCode = 404;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no método {Method} | ID: {ID} | DTO: {@Entidade} | UsuarioId: {UsuarioId}", nameof(this.UpdateAsync), id, categoriaDto, usuarioId);
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
                Categoria categoria = await _repository.GetCategoriaById(id, usuarioId) ?? throw new NullReferenceException("A categoria não existe!");

                if (categoria.Padrao)
                {
                    response.Sucesso = false;
                    response.Erro = "Não é possível excluir uma categoria padrão do sistema!";
                    response.StatusCode = 401;
                    return response;
                }

                _repository.Delete(categoria);

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

        public async Task<ResponseCategoriaDto?> GetCategoriaByIdAsync(int id, int usuarioId)
        {
            return _mapper.Map<ResponseCategoriaDto>(await _repository.GetCategoriaById(id, usuarioId));
        }

        public async Task<IEnumerable<ResponseCategoriaDto>> GetCategoriasAsync(int usuarioId)
        {
            return _mapper.Map<IEnumerable<ResponseCategoriaDto>>(await _repository.GetCategorias(usuarioId));
        }
    }
}