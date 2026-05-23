using AutoMapper;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Lancamento;
using MoneyAPI.Repositories.Interfaces;
using MoneyAPI.Services.Interfaces;

namespace MoneyAPI.Services
{
    public class LancamentoService : ILancamentoService
    {
        private readonly ILancamentoRepository _repository;
        private readonly IMapper _mapper;

        public LancamentoService(ILancamentoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ResponseDto> CreateAsync(RequestLancamentoDto lancamentoDto, int usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdateAsync(int id, RequestLancamentoDto lancamentoDto, int usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> DeleteAsync(int id, int usuarioId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseLancamentoDto> GetLancamentoByIdAsync(int id, int usuarioId)
        {
            return _mapper.Map<ResponseLancamentoDto>(await _repository.GetLancamentoById(id, usuarioId));
        }

        public async Task<IEnumerable<ResponseLancamentoDto>> GetLancamentosMensalAsync(int usuarioId, int mes, int ano)
        {
            return _mapper.Map<IEnumerable<ResponseLancamentoDto>>(await _repository.GetLancamentosMensal(usuarioId, mes, ano));
        }
    }
}