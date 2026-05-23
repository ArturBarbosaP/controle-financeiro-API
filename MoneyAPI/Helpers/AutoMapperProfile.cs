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

            /*CreateMap<AddUsuarioDto, Usuario>()
                .ForMember(dest => dest.Categorias, opt => opt.Ignore())
                .ForMember(dest => dest.Contas, opt => opt.Ignore())
                .ForMember(dest => dest.Lancamentos, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Senha, opt =>
                {   //ignora se a senha chegar null
                    opt.PreCondition(src => !string.IsNullOrEmpty(src.Senha));
                    opt.MapFrom(src => PasswordHelper.HashPassword(src.Senha));
                });

            CreateMap<Usuario, AddUsuarioDto>()
                .ForMember(dest => dest.Senha, opt => opt.Ignore());*/

            /*CreateMap<RequestAddUsuarioDto, Usuario>()
                .ForMember(dest => dest.Categorias, opt => opt.Ignore())
                .ForMember(dest => dest.Contas, opt => opt.Ignore())
                .ForMember(dest => dest.Lancamentos, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Senha, opt => opt.MapFrom(src => PasswordHelper.HashPassword(src.Senha)));

            CreateMap<RequestUpdateUsuarioDto, Usuario>()
                .ForMember(dest => dest.Categorias, opt => opt.Ignore())
                .ForMember(dest => dest.Contas, opt => opt.Ignore())
                .ForMember(dest => dest.Lancamentos, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Senha, opt => opt.Ignore()); */

            CreateMap<RequestAddUsuarioDto, Usuario>()
                .ForMember(dest => dest.Senha, opt => opt.MapFrom(src => PasswordHelper.HashPassword(src.Senha)));

            CreateMap<RequestUpdateUsuarioDto, Usuario>();

            CreateMap<RequestPasswordUpdateUsuarioDto, Usuario>()
                .ForMember(dest => dest.Senha, opt => opt.MapFrom(src => PasswordHelper.HashPassword(src.NovaSenha)));

            CreateMap<Usuario, ResponseUsuarioDto>();

            #endregion

            #region Categoria

            CreateMap<Categoria, RequestCategoriaDto>()
                .ReverseMap();

            CreateMap<Categoria, ResponseCategoriaDto>();

            #endregion

            #region Conta

            CreateMap<Conta, RequestContaDto>()
                .ReverseMap();

            CreateMap<Conta, ResponseContaDto>();

            #endregion

            #region Cartão

            CreateMap<Cartao, RequestCartaoDto>()
                .ReverseMap();

            CreateMap<Cartao, ResponseCartaoDto>()
                .ForMember(dest => dest.ContaNome, opt => opt.MapFrom(src => src.Conta.Nome));

            #endregion

            #region Limite

            CreateMap<Limite, RequestLimiteDto>()
                .ReverseMap();

            CreateMap<Limite, ResponseLimiteDto>()
                .ForMember(dest => dest.CategoriaNome, opt => opt.MapFrom(src => src.Categoria.Nome));

            #endregion

            CreateMap<Lancamento, LancamentoDto>()
                .ForMember(dest => dest.CategoriaNome, opt => opt.MapFrom(src => src.Categoria.Nome))
                .ForMember(dest => dest.CartaoNome, opt => opt.MapFrom(src => src.Cartao.Nome))
                .ForMember(dest => dest.ContaNome, opt => opt.MapFrom(src => src.Conta.Nome));
        }
    }
}