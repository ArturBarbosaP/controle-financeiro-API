using AutoMapper;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Cartao;
using MoneyAPI.Models.Entities;
using MoneyAPI.Repositories.Interfaces;
using MoneyAPI.Services.Interfaces;

namespace MoneyAPI.Services
{
    public class CartaoService : ICartaoService
    {
        private readonly ICartaoRepository _repository;
        private readonly IContaRepository _contaRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly IMapper _mapper;

        public CartaoService(ICartaoRepository repository, IContaRepository contaRepository, ICategoriaRepository categoriaRepository, ILancamentoRepository lancamentoRepository, IMapper mapper)
        {
            _repository = repository;
            _contaRepository = contaRepository;
            _categoriaRepository = categoriaRepository;
            _lancamentoRepository = lancamentoRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateAsync(RequestCartaoDto cartaoDto, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                Conta conta = await _contaRepository.GetContaById(cartaoDto.ContaId, usuarioId) ?? throw new NullReferenceException("Conta não encontrada!");

                if (await _repository.GetCartaoByNome(cartaoDto.Nome, usuarioId) != null) //bloquear se ja existir cartao com o mesmo nome
                {
                    response.Sucesso = false;
                    response.Erro = "Já existe um cartão com esse nome!";
                    response.StatusCode = 400;
                    return response;
                }

                Cartao cartaoInsert = _mapper.Map<Cartao>(cartaoDto);

                _repository.Add(cartaoInsert);

                if (!await _repository.SaveChanges())
                    throw new Exception("Não foi possível criar no banco!");

                response.Sucesso = true;
                response.Entidade = _mapper.Map<ResponseCartaoDto>(cartaoInsert);
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

        public async Task<ResponseDto> UpdateAsync(int id, RequestCartaoDto cartaoDto, int usuarioId)
        {
            ResponseDto response = new();

            try
            {
                Cartao cartao = await _repository.GetCartaoById(id, usuarioId) ?? throw new NullReferenceException("O cartão não existe!");

                bool alteracaoNome = cartaoDto.Nome != cartao.Nome;

                if (alteracaoNome)
                {
                    if (await _repository.GetCartaoByNome(cartaoDto.Nome, usuarioId) != null)
                    {
                        //bloquear se ja existir cartao com o mesmo nome, caso o nome seja alterado
                        response.Sucesso = false;
                        response.Erro = "Já existe um cartão com esse nome!";
                        response.StatusCode = 400;
                        return response;
                    }

                    Categoria categoriaFatura = await _categoriaRepository.GetCategoriaPadraoFatura(usuarioId);

                    IEnumerable<Lancamento> lancamentos = await _lancamentoRepository.GetLancamentosFaturasCartao(cartao.Nome, cartao.ContaId, categoriaFatura.Id, usuarioId);

                    foreach (Lancamento l in lancamentos)
                    {
                        l.Descricao = cartaoDto.Nome;
                        _lancamentoRepository.Update(l); //atualizando o novo nome nas faturas
                    }
                }

                Cartao cartaoUpdate = _mapper.Map(cartaoDto, cartao);
                _repository.Update(cartaoUpdate);

                if (!await _repository.SaveChanges())
                    throw new Exception("Não foi possível atualizar no banco!");

                response.Sucesso = true;
                response.Entidade = _mapper.Map<ResponseCartaoDto>(cartaoUpdate);
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
                response.Erro = ex.Message + "\n" + ex.InnerException;
                response.StatusCode = 500;
            }

            return response;
        }

        public async Task<ResponseCartaoDto?> GetCartaoByIdAsync(int id, int usuarioId)
        {
            return _mapper.Map<ResponseCartaoDto>(await _repository.GetCartaoById(id, usuarioId));
        }

        public async Task<IEnumerable<ResponseCartaoDto>> GetCartoesAsync(int usuarioId)
        {
            return _mapper.Map<IEnumerable<ResponseCartaoDto>>(await _repository.GetCartoes(usuarioId));
        }

        public async Task ResetarFatura() //pr_ResetarFatura no banco antigo
        {
            List<Cartao> cartoes = await _repository.GetCartoesFechados();

            foreach (Cartao cartao in cartoes)
            {
                DateOnly dataInicio = cartao.DataFechamento.AddMonths(-1).AddDays(1);
                decimal valorParcelado = await _lancamentoRepository.GetLancamentosParceladosNaFatura(cartao.Id, dataInicio, cartao.DataFechamento);
                decimal valorFatura = await _lancamentoRepository.GetLancamentosNaFatura(cartao.Id, dataInicio, cartao.DataFechamento);

                cartao.DataFechamento = cartao.DataFechamento.AddMonths(1);
                cartao.DataVencimento = cartao.DataVencimento.AddMonths(1);

                cartao.ValorParcelado += valorParcelado;
                cartao.LimiteDisponivel = cartao.Limite + valorFatura - cartao.ValorParcelado;

                _repository.Update(cartao);
            }

            if (!await _repository.SaveChanges())
                throw new Exception("Não foi possível alterar no banco no banco!");
        }
    }
}