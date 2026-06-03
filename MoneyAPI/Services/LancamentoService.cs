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
                Cartao? cartao = null;
                Conta? contaDestino = null;

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
                    await InsertParcelado(lancamentoDto, usuarioId, conta, contaDestino, cartao);
                }
                else if (lancamentoDto.Fixo) //lancamento fixo
                {
                    await InsertFixo(lancamentoDto, usuarioId, conta, contaDestino, cartao);
                }
                else //lancamento normal
                {
                    Lancamento lancamentoInsert = _mapper.Map<Lancamento>(lancamentoDto);
                    lancamentoInsert.UsuarioId = usuarioId;

                    _repository.Add(lancamentoInsert);

                    if (lancamentoInsert.CartaoId == null) //se o lancamento tiver cartao, atualiza o limite e o valor da fatura
                        AtualizarSaldo(lancamentoInsert.Valor, lancamentoInsert.PreLancamento, conta, contaDestino); //se o lancamento for de transferencia, ja altera o saldo nas duas contas
                    else
                        await AtualizarCartao(lancamentoInsert, cartao!, usuarioId, false);

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

        public async Task<ResponseDto> DeleteAsync(int id, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                Lancamento lancamento = await _repository.GetLancamentoById(id, usuarioId) ?? throw new NullReferenceException("O lançamento não existe!");
                
                Conta conta = await _contaRepository.GetContaById(lancamento.ContaId, usuarioId) ?? throw new NullReferenceException("Conta não encontrada!");
                
                Cartao? cartao = lancamento.CartaoId != null ? await _cartaoRepository.GetCartaoById((int)lancamento.CartaoId, usuarioId) ?? throw new NullReferenceException("Cartão não encontrado!") : null;
                Conta? contaDestino = lancamento.ContaDestinoId != null ? await _contaRepository.GetContaById((int)lancamento.ContaDestinoId, usuarioId) ?? throw new NullReferenceException("Conta destino não encontrada!") : null;

                if (cartao == null) //apenas voltar com o valor do lancamento pro saldo
                    AtualizarSaldo(contaDestino == null ? lancamento.Valor * -1 : lancamento.Valor, lancamento.PreLancamento, conta, contaDestino); //mantendo o valor pra lancamento de transferencia
                else
                    await AtualizarCartao(lancamento, cartao, usuarioId, false, true);

                _repository.Delete(lancamento);

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
                response.Erro = ex.Message + "\n" + ex.InnerException;
                response.StatusCode = 500;
            }

            return response;
        }

        public async Task<ResponseDto> DeleteFixoAsync(int id, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                Lancamento lancamento = await _repository.GetLancamentoById(id, usuarioId) ?? throw new NullReferenceException("O lançamento não existe!");

                if (!lancamento.Fixo)
                {
                    response.Sucesso = false;
                    response.Erro = "O lançamento não é Fixo!";
                    response.StatusCode = 400;
                    return response;
                }

                Conta conta = await _contaRepository.GetContaById(lancamento.ContaId, usuarioId) ?? throw new NullReferenceException("Conta não encontrada!");

                Cartao? cartao = lancamento.CartaoId != null ? await _cartaoRepository.GetCartaoById((int)lancamento.CartaoId, usuarioId) ?? throw new NullReferenceException("Cartão não encontrado!") : null;
                Conta? contaDestino = lancamento.ContaDestinoId != null ? await _contaRepository.GetContaById((int)lancamento.ContaDestinoId, usuarioId) ?? throw new NullReferenceException("Conta destino não encontrada!") : null;

                List<Lancamento> lancamentos = await _repository.GetLancamentosFixosByLancamento(lancamento, usuarioId);

                if (lancamentos.Count == 0)
                    throw new NullReferenceException("O lançamento não existe!");

                foreach (Lancamento l in lancamentos)
                {
                    if (cartao == null) //apenas voltar com o valor do lancamento pro saldo
                        AtualizarSaldo(contaDestino == null ? l.Valor * -1 : l.Valor, l.PreLancamento, conta, contaDestino); //mantendo o valor pra lancamento de transferencia
                    else
                        await AtualizarCartao(l, cartao, usuarioId, false, true);

                    _repository.Delete(l);
                }

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
                response.Erro = ex.Message + "\n" + ex.InnerException;
                response.StatusCode = 500;
            }

            return response;
        }

        public async Task<ResponseLancamentoDto> GetLancamentoByIdAsync(int id, int usuarioId)
        {
            return _mapper.Map<ResponseLancamentoDto>(await _repository.GetLancamentoById(id, usuarioId));
        }

        public async Task<IEnumerable<ResponseLancamentoDto>> GetLancamentosMensalAsync(int usuarioId, int mes, int ano)
        {
            return _mapper.Map<IEnumerable<ResponseLancamentoDto>>(await _repository.GetLancamentosMensal(usuarioId, mes, ano));
        }

        #region Triggers e procedures do legado

        private async Task InsertParcelado(RequestLancamentoDto lancamentoDto, int usuarioId, Conta conta, Conta? contaDestino, Cartao? cartao) //pr_AdicionarParcelado no banco antigo
        {
            bool ultimoDiaMes = lancamentoDto.Data.Day == DateTime.DaysInMonth(lancamentoDto.Data.Year, lancamentoDto.Data.Month);
            decimal valorParcela = Math.Round(lancamentoDto.Valor / lancamentoDto.Parcelas, 2, MidpointRounding.AwayFromZero);
            decimal valorPrimeira = lancamentoDto.Valor - (valorParcela * (lancamentoDto.Parcelas - 1));

            if (lancamentoDto.CartaoId != null) //se tiver cartao, atualiza o limite e o valor parcelado
            {
                AtualizarLimite(lancamentoDto.Valor, cartao!);
                AtualizarValorParcelado(lancamentoDto.Valor, cartao!);
            }

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
                CalcularPreLancamento(lancamentoInsert);

                _repository.Add(lancamentoInsert);

                if (lancamentoInsert.CartaoId == null) //se o lancamento tiver cartao, atualiza o limite e o valor da fatura
                    AtualizarSaldo(lancamentoInsert.Valor, lancamentoInsert.PreLancamento, conta, contaDestino);
                else
                    await AtualizarCartao(lancamentoInsert, cartao!, usuarioId, true);
            }
        }

        private async Task InsertFixo(RequestLancamentoDto lancamentoDto, int usuarioId, Conta conta, Conta? contaDestino, Cartao? cartao) //pr_AdicionarFixo no banco antigo
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
                CalcularPreLancamento(lancamentoInsert);

                _repository.Add(lancamentoInsert);

                if (lancamentoInsert.CartaoId == null) //se o lancamento tiver cartao, atualiza o limite e o valor da fatura
                    AtualizarSaldo(lancamentoInsert.Valor, lancamentoInsert.PreLancamento, conta, contaDestino);
                else
                    await AtualizarCartao(lancamentoInsert, cartao!, usuarioId, false);
            }
        }

        private void AtualizarSaldo(decimal valor, bool preLancamento, Conta conta, Conta? contaDestino) //pr_AlterarSaldo no banco antigo
        {
            if (preLancamento) //atualiza saldo apenas se o lancamento for de hoje ou mais antigo
                return;

            conta.Saldo += valor;

            if (contaDestino != null) //colocando saldo na conta destino se o lancamento for de transferencia
            {
                contaDestino.Saldo += valor * -1;
                _contaRepository.Update(contaDestino);
            }

            _contaRepository.Update(conta);
        }

        private void AtualizarLimite(decimal valor, Cartao cartao)
        {
            cartao.LimiteDisponivel += valor;
            _cartaoRepository.Update(cartao);
        }

        private void AtualizarValorParcelado(decimal valor, Cartao cartao)
        {
            cartao.ValorParcelado += valor * -1;
            _cartaoRepository.Update(cartao);
        }

        private async Task AtualizarCartao(Lancamento lancamento, Cartao cartao, int usuarioId, bool parcelado, bool excluir = false) //pr_InserirFatura e pr_AlterarFatura no banco antigo
        {
            decimal valorOperacao = excluir ? lancamento.Valor * -1 : lancamento.Valor;

            Categoria categoriaFatura = await _categoriaRepository.GetCategoriaPadraoFatura(usuarioId);
            DateOnly dataFinalFatura = cartao.DataFechamento;
            DateOnly dataInicioFatura = cartao.DataFechamento.AddMonths(-1).AddDays(1);
            int addMonths = 0;

            if (!parcelado && (lancamento.Data >= dataInicioFatura && lancamento.Data <= dataFinalFatura)) //fatura atual
            {
                AtualizarLimite(valorOperacao, cartao);
            }
            else if (lancamento.Data > dataFinalFatura)
            {
                while (!(lancamento.Data >= dataInicioFatura && lancamento.Data <= dataFinalFatura))
                {
                    addMonths++;
                    dataInicioFatura = dataInicioFatura.AddMonths(1);
                    dataFinalFatura = dataFinalFatura.AddMonths(1);
                }
            }
            else if (lancamento.Data < dataInicioFatura)
            {
                while (!(lancamento.Data >= dataInicioFatura && lancamento.Data <= dataFinalFatura))
                {
                    addMonths--;
                    dataInicioFatura = dataInicioFatura.AddMonths(-1);
                    dataFinalFatura = dataFinalFatura.AddMonths(-1);
                }
            }

            Lancamento fatura = await _repository.GetLancamentoFaturaCartao(cartao.Nome, cartao.DataVencimento.AddMonths(addMonths), cartao.ContaId, categoriaFatura.Id, usuarioId);

            if (fatura == null) //sem lancamento de fatura, adicionando um
            {
                if (excluir)
                    throw new Exception($"Fatura do cartão {cartao.Nome} não encontrada!");

                Lancamento lancamentoInsert = new()
                {
                    CategoriaId = categoriaFatura.Id,
                    ContaId = cartao.ContaId,
                    Tipo = "Despesa",
                    Valor = lancamento.Valor,
                    Descricao = cartao.Nome,
                    Data = cartao.DataVencimento.AddMonths(addMonths),
                    Observacao = $"Data de fechamento: {cartao.DataFechamento.ToString("dd/MM/yyyy")}",
                    UsuarioId = usuarioId
                };

                CalcularPreLancamento(lancamentoInsert);

                _repository.Add(lancamentoInsert);

                //atualizando saldo direto do lancamento inserido
                AtualizarSaldo(lancamentoInsert.Valor, lancamentoInsert.PreLancamento, cartao.Conta, null);
            }
            else
            {
                fatura.Valor += valorOperacao;
                _repository.Update(fatura);

                //tirando o valor do lancamento na fatura, atualizando saldo se a fatura for pre lancamento
                AtualizarSaldo(valorOperacao, fatura.PreLancamento, cartao.Conta, null);
            }
        }

        #endregion

        #region Auxiliares

        private void NormalizarLancamento(RequestLancamentoDto lancamentoDto)
        {
            CalcularPreLancamento(lancamentoDto);

            lancamentoDto.Valor = lancamentoDto.Tipo == "Receita" ? lancamentoDto.Valor : lancamentoDto.Valor * -1;

            lancamentoDto.Parcelas = lancamentoDto.Parcelas <= 1 ? 0 : lancamentoDto.Parcelas;

            lancamentoDto.CartaoId = lancamentoDto.CartaoId == 0 ? null : lancamentoDto.CartaoId;

            lancamentoDto.ContaDestinoId = lancamentoDto.ContaDestinoId == 0 ? null : lancamentoDto.ContaDestinoId;

            lancamentoDto.Observacao = string.IsNullOrWhiteSpace(lancamentoDto.Observacao) ? null : lancamentoDto.Observacao;
        }

        private void CalcularPreLancamento(RequestLancamentoDto lancamentoDto)
        {
            lancamentoDto.PreLancamento = lancamentoDto.Data > DateOnly.FromDateTime(DateTime.Now);
        }

        private void CalcularPreLancamento(Lancamento lancamento)
        {
            lancamento.PreLancamento = lancamento.Data > DateOnly.FromDateTime(DateTime.Now);
        }

        #endregion
    }
}