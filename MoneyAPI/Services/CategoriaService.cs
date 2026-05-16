using AutoMapper;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.Entities;
using MoneyAPI.Repositories.Interfaces;
using MoneyAPI.Services.Interfaces;

namespace MoneyAPI.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;
        private readonly IMapper _mapper;

        public CategoriaService(ICategoriaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateAsync(CategoriaDto categoriaDto, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                Categoria categoriaInsert = _mapper.Map<Categoria>(categoriaDto);
                categoriaInsert.UsuarioId = usuarioId;

                _repository.Add(categoriaInsert);

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

        public async Task<ResponseDto> UpdateAsync(int id, CategoriaDto categoriaDto, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                Categoria categoria = await _repository.GetCategoriaById(id, usuarioId) ?? throw new NullReferenceException("A categoria não existe!");

                Categoria categoriaUpdate = _mapper.Map(categoriaDto, categoria);
                _repository.Update(categoriaUpdate);

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
                Categoria categoria = await _repository.GetCategoriaById(id, usuarioId) ?? throw new NullReferenceException("A categoria não existe!");

                _repository.Delete(categoria);

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

        public async Task<CategoriaDto?> GetCategoriaByIdAsync(int id, int usuarioId)
        {
            return _mapper.Map<CategoriaDto>(await _repository.GetCategoriaById(id, usuarioId));
        }

        public async Task<IEnumerable<CategoriaDto>> GetCategoriasAsync(int usuarioId)
        {
            return _mapper.Map<IEnumerable<CategoriaDto>>(await _repository.GetCategorias(usuarioId));
        }
    }
}