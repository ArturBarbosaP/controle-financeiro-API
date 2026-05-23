using AutoMapper;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Cartao;
using MoneyAPI.Models.DTOs.Categoria;
using MoneyAPI.Models.DTOs.Conta;
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
                .ForMember(dest => dest.ContaNome, opt => opt.MapFrom(src => src.Conta.Nome));

            #endregion

            #region Limite

            CreateMap<RequestLimiteDto, Limite>();

            CreateMap<Limite, ResponseLimiteDto>()
                .ForMember(dest => dest.CategoriaNome, opt => opt.MapFrom(src => src.Categoria.Nome));

            #endregion

            #region Lançamento

            CreateMap<Lancamento, LancamentoDto>()
                .ForMember(dest => dest.CategoriaNome, opt => opt.MapFrom(src => src.Categoria.Nome))
                .ForMember(dest => dest.CartaoNome, opt => opt.MapFrom(src => src.Cartao.Nome))
                .ForMember(dest => dest.ContaNome, opt => opt.MapFrom(src => src.Conta.Nome));

            #endregion
        }
    }
}