using AutoMapper;
using MoneyAPI.Models.DTOs.Cartao;
using MoneyAPI.Models.DTOs.Categoria;
using MoneyAPI.Models.DTOs.Conta;
using MoneyAPI.Models.DTOs.Lancamento;
using MoneyAPI.Models.DTOs.Limite;
using MoneyAPI.Models.DTOs.Usuario;
using MoneyAPI.Models.Entities;

namespace MoneyAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Usuario

            CreateMap<RequestAddUsuarioDto, Usuario>()
                .ForMember(dest => dest.Senha, opt => opt.MapFrom(src => PasswordHelper.HashPassword(src.Senha)));

            CreateMap<RequestUpdateUsuarioDto, Usuario>();

            CreateMap<RequestPasswordUpdateUsuarioDto, Usuario>()
                .ForMember(dest => dest.Senha, opt => opt.MapFrom(src => PasswordHelper.HashPassword(src.NovaSenha)));

            CreateMap<Usuario, ResponseUsuarioDto>();

            #endregion

            #region Categoria

            CreateMap<RequestCategoriaDto, Categoria>();

            CreateMap<Categoria, ResponseCategoriaDto>();

            #endregion

            #region Conta

            CreateMap<RequestContaDto, Conta>();

            CreateMap<Conta, ResponseContaDto>();

            #endregion

            #region Cartão

            CreateMap<RequestCartaoDto, Cartao>();

            CreateMap<Cartao, ResponseCartaoDto>()
                .ForMember(dest => dest.ContaNome, opt => opt.MapFrom(src => src.Conta.Nome))
                .ForMember(dest => dest.Fatura,
                            opt => opt.MapFrom(src => src.Lancamentos
                                    .Where(l =>
                                        l.Data >= src.DataFechamento.AddMonths(-1).AddDays(1) &&
                                        l.Data <= src.DataFechamento
                                    )
                                    .Sum(l => l.Valor)
                            )
                );

            #endregion

            #region Limite

            CreateMap<RequestLimiteDto, Limite>();

            CreateMap<Limite, ResponseLimiteDto>()
                .ForMember(dest => dest.CategoriaNome, opt => opt.MapFrom(src => src.Categoria.Nome))
                .ForMember(dest => dest.CategoriaCor, opt => opt.MapFrom(src => src.Categoria.Cor));

            #endregion

            #region Lançamento

            CreateMap<RequestLancamentoDto, Lancamento>();

            CreateMap<Lancamento, ResponseLancamentoDto>()
                .ForMember(dest => dest.CategoriaNome, opt => opt.MapFrom(src => src.Categoria.Nome))
                .ForMember(dest => dest.CategoriaCor, opt => opt.MapFrom(src => src.Categoria.Cor))
                .ForMember(dest => dest.CartaoNome, opt => opt.MapFrom(src => src.Cartao.Nome))
                .ForMember(dest => dest.ContaNome, opt => opt.MapFrom(src => src.Conta.Nome))
                .ForMember(dest => dest.ContaDestinoNome, opt => opt.MapFrom(src => src.ContaDestino.Nome));

            #endregion
        }
    }
}