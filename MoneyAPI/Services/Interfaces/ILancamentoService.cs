using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Lancamento;

namespace MoneyAPI.Services.Interfaces
{
    public interface ILancamentoService
    {
        Task<IEnumerable<ResponseLancamentoDto>> GetLancamentosMensalAsync(int usuarioId, int mes, int ano);
        Task<ResponseLancamentoDto> GetLancamentoByIdAsync(int id, int usuarioId);
        Task<ResponseDto> CreateAsync(RequestLancamentoDto lancamentoDto, int usuarioId);
        Task<ResponseDto> UpdateAsync(int id, RequestLancamentoDto lancamentoDto, int usuarioId);
        Task<ResponseDto> UpdateFixoAsync(int id, RequestLancamentoDto lancamentoDto, int usuarioId);
        Task<ResponseDto> DeleteAsync(int id, int usuarioId);
        Task<ResponseDto> DeleteFixoAsync(int id, int usuarioId);
    }
}