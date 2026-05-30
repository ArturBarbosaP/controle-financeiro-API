using AutoMapper;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Lancamento;
using MoneyAPI.Models.Entities;
using MoneyAPI.Repositories.Interfaces;
using MoneyAPI.Services.Interfaces;

namespace MoneyAPI.Services
{
    public class LancamentoService : ILancamentoService
    {
        private readonly ILancamentoRepository _repository;
        private readonly IContaRepository _contaRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ICartaoRepository _cartaoRepository;
        private readonly IMapper _mapper;

        public LancamentoService(ILancamentoRepository repository, IContaRepository contaRepository, ICategoriaRepository categoriaRepository, ICartaoRepository cartaoRepository, IMapper mapper)
        {
            _repository = repository;
            _contaRepository = contaRepository;
            _categoriaRepository = categoriaRepository;
            _cartaoRepository = cartaoRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateAsync(RequestLancamentoDto lancamentoDto, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                NormalizarLancamento(lancamentoDto);

                Conta conta = await _contaRepository.GetContaById(lancamentoDto.ContaId, usuarioId) ?? throw new NullReferenceException("Conta não encontrada!");
                Categoria categoria = await _categoriaRepository.GetCategoriaByIdTipo(lancamentoDto.CategoriaId, lancamentoDto.Tipo, usuarioId) ?? throw new NullReferenceException("Categoria não encontrada!");
                Cartao cartao = null;
                Conta contaDestino = null;

                if (lancamentoDto.CartaoId != null)
                {
                    cartao = await _cartaoRepository.GetCartaoById((int)lancamentoDto.CartaoId, usuarioId) ?? throw new NullReferenceException("Cartão não encontrado!");
                }

                if (lancamentoDto.ContaDestinoId != null) //transferencia
                {
                    contaDestino = await _contaRepository.GetContaById((int)lancamentoDto.ContaDestinoId, usuarioId) ?? throw new NullReferenceException("Conta destino não encontrada!");
                    string obs = $"{conta.Nome} -> {contaDestino.Nome}";
                    lancamentoDto.Observacao = lancamentoDto.Observacao != null ? obs + " - " + lancamentoDto.Observacao.TrimStart() : obs;
                }

                if (lancamentoDto.Parcelas != 0) //lancamento parcelado
                {
                    InsertParcelado(lancamentoDto, usuarioId, conta, contaDestino);
                }
                else if (lancamentoDto.Fixo) //lancamento fixo
                {
                    InsertFixo(lancamentoDto, usuarioId, conta, contaDestino);
                }
                else //lancamento normal
                {
                    Lancamento lancamentoInsert = _mapper.Map<Lancamento>(lancamentoDto);
                    lancamentoInsert.UsuarioId = usuarioId;

                    _repository.Add(lancamentoInsert);

                    if (lancamentoInsert.CartaoId == null) //se o lancamento tiver cartao, atualiza o limite e o valor da fatura
                        AtualizarSaldo(lancamentoInsert, conta, contaDestino); //se o lancamento for de transferencia, ja altera o saldo nas duas contas
                    else
                        AtualizarCartao(lancamentoInsert);

                    response.Entidade = _mapper.Map<ResponseLancamentoDto>(lancamentoInsert);
                }

                if (!await _repository.SaveChanges())
                    throw new Exception("Não foi possível criar no banco!");

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
                response.Erro = ex.Message + "\n" + ex.InnerException;
                response.StatusCode = 500;
            }

            return response;
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

        private void NormalizarLancamento(RequestLancamentoDto lancamentoDto)
        {
            lancamentoDto.PreLancamento = lancamentoDto.Data > DateOnly.FromDateTime(DateTime.Now);

            lancamentoDto.Valor = lancamentoDto.Tipo == "Receita" ? lancamentoDto.Valor : lancamentoDto.Valor * -1;

            lancamentoDto.Parcelas = lancamentoDto.Parcelas <= 1 ? 0 : lancamentoDto.Parcelas;

            lancamentoDto.CartaoId = lancamentoDto.CartaoId == 0 ? null : lancamentoDto.CartaoId;

            lancamentoDto.Observacao = string.IsNullOrWhiteSpace(lancamentoDto.Observacao) ? null : lancamentoDto.Observacao;
        }


        private void InsertParcelado(RequestLancamentoDto lancamentoDto, int usuarioId, Conta conta, Conta? contaDestino) //pr_AdicionarParcelado no banco antigo
        {
            bool ultimoDiaMes = lancamentoDto.Data.Day == DateTime.DaysInMonth(lancamentoDto.Data.Year, lancamentoDto.Data.Month);
            decimal valorParcela = Math.Round(lancamentoDto.Valor / lancamentoDto.Parcelas, 2, MidpointRounding.AwayFromZero);
            decimal valorPrimeira = lancamentoDto.Valor - (valorParcela * (lancamentoDto.Parcelas - 1));

            for (int i = 1; i <= lancamentoDto.Parcelas; i++)
            {
                Lancamento lancamentoInsert = _mapper.Map<Lancamento>(lancamentoDto);
                lancamentoInsert.UsuarioId = usuarioId;
                lancamentoInsert.Descricao = $"{i}/{lancamentoDto.Parcelas} {lancamentoDto.Descricao}";
                lancamentoInsert.Valor = i == 1 ? valorPrimeira : valorParcela;

                DateOnly novaData = lancamentoDto.Data.AddMonths(i - 1);
                int dia = ultimoDiaMes
                    ? DateTime.DaysInMonth(novaData.Year, novaData.Month)
                    : Math.Min(lancamentoDto.Data.Day, DateTime.DaysInMonth(novaData.Year, novaData.Month)); //caso o dia do mes selecionado for maior que o ultimo dia de algum mes

                lancamentoInsert.Data = new DateOnly(novaData.Year, novaData.Month, dia);

                _repository.Add(lancamentoInsert);

                if (lancamentoInsert.CartaoId == null) //se o lancamento tiver cartao, atualiza o limite e o valor da fatura
                    AtualizarSaldo(lancamentoInsert, conta, contaDestino);
                else
                    AtualizarCartao(lancamentoInsert);
            }
        }

        private void InsertFixo(RequestLancamentoDto lancamentoDto, int usuarioId, Conta conta, Conta? contaDestino) //pr_AdicionarFixo no banco antigo
        {
            bool ultimoDiaMes = lancamentoDto.Data.Day == DateTime.DaysInMonth(lancamentoDto.Data.Year, lancamentoDto.Data.Month);

            for (int mes = lancamentoDto.Data.Month; mes <= 12; mes++)
            {
                Lancamento lancamentoInsert = _mapper.Map<Lancamento>(lancamentoDto);
                lancamentoInsert.UsuarioId = usuarioId;

                int dia = ultimoDiaMes
                    ? DateTime.DaysInMonth(lancamentoDto.Data.Year, mes)
                    : Math.Min(lancamentoDto.Data.Day, DateTime.DaysInMonth(lancamentoDto.Data.Year, mes)); //caso o dia do mes selecionado for maior que o ultimo dia de algum mes

                lancamentoInsert.Data = new DateOnly(lancamentoDto.Data.Year, mes, dia);

                _repository.Add(lancamentoInsert);

                if (lancamentoInsert.CartaoId == null) //se o lancamento tiver cartao, atualiza o limite e o valor da fatura
                    AtualizarSaldo(lancamentoInsert, conta, contaDestino);
                else
                    AtualizarCartao(lancamentoInsert);
            }
        }

        private void AtualizarSaldo(Lancamento lancamento, Conta conta, Conta? contaDestino) //pr_AlterarSaldo no banco antigo
        {
            if (lancamento.PreLancamento) //atualiza saldo apenas se o lancamento for de hoje ou mais antigo
                return;

            conta.Saldo += lancamento.Valor;

            if (contaDestino != null) //colocando saldo na conta destino se o lancamento for de transferencia
            {
                contaDestino.Saldo += lancamento.Valor * -1;
                _contaRepository.Update(contaDestino);
            }

            _contaRepository.Update(conta);
        }

        private void AtualizarCartao(Lancamento lancamento)
        {
            throw new NotImplementedException();
        }
    }
}