using AutoMapper;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.Entities;
using MoneyAPI.Repositories.Interfaces;
using MoneyAPI.Services.Interfaces;

namespace MoneyAPI.Services
{
    public class CartaoService : ICartaoService
    {
        private readonly ICartaoRepository _repository;
        private readonly IMapper _mapper;

        public CartaoService(ICartaoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateAsync(CartaoDto cartaoDto, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                Cartao cartaoInsert = _mapper.Map<Cartao>(cartaoDto);

                _repository.Add(cartaoInsert);

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

        public async Task<ResponseDto> UpdateAsync(int id, CartaoDto cartaoDto, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                Cartao cartao = await _repository.GetCartaoById(id, usuarioId) ?? throw new NullReferenceException("O cartão não existe!");

                Cartao cartaoUpdate = _mapper.Map(cartaoDto, cartao);
                _repository.Update(cartaoUpdate);

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
                Cartao cartao = await _repository.GetCartaoById(id, usuarioId) ?? throw new NullReferenceException("O cartão não existe!");

                _repository.Delete(cartao);

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

        public async Task<CartaoDto?> GetCartaoByIdAsync(int id, int usuarioId)
        {
            return _mapper.Map<CartaoDto>(await _repository.GetCartaoById(id, usuarioId));
        }

        public async Task<IEnumerable<CartaoDto>> GetCartoesAsync(int usuarioId)
        {
            return _mapper.Map<IEnumerable<CartaoDto>>(await _repository.GetCartoes(usuarioId));
        }
    }
}