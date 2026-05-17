using AutoMapper;
using MoneyAPI.Models.DTOs;
using MoneyAPI.Models.DTOs.Usuario;
using MoneyAPI.Models.Entities;

namespace MoneyAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
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

            CreateMap<AddUsuarioDto, Usuario>()
                .ForMember(dest => dest.Categorias, opt => opt.Ignore())
                .ForMember(dest => dest.Contas, opt => opt.Ignore())
                .ForMember(dest => dest.Lancamentos, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Senha, opt => opt.MapFrom(src => PasswordHelper.HashPassword(src.Senha)));

            CreateMap<UpdateUsuarioDto, Usuario>()
                .ForMember(dest => dest.Categorias, opt => opt.Ignore())
                .ForMember(dest => dest.Contas, opt => opt.Ignore())
                .ForMember(dest => dest.Lancamentos, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Senha, opt => opt.Ignore());

            CreateMap<CategoriaDto, Categoria>()
                .ForMember(dest => dest.Lancamentos, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioId, opt => opt.Ignore())
                .ForMember(dest => dest.Limite, opt => opt.Ignore());

            CreateMap<Usuario, ReadUsuarioDto>();

            CreateMap<Conta, ContaDto>();

            CreateMap<ContaDto, Conta>()
                .ForMember(dest => dest.Cartoes, opt => opt.Ignore())
                .ForMember(dest => dest.Lancamentos, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioId, opt => opt.Ignore());

            CreateMap<Cartao, CartaoDto>()
                .ForMember(dest => dest.ContaNome, opt => opt.MapFrom(src => src.Conta.Nome));

            CreateMap<CartaoDto, Cartao>()
                .ForMember(dest => dest.Lancamentos, opt => opt.Ignore())
                .ForMember(dest => dest.Conta, opt => opt.Ignore());

            CreateMap<Limite, LimiteDto>()
                .ForMember(dest => dest.CategoriaNome, opt => opt.MapFrom(src => src.Categoria.Nome));

            CreateMap<LimiteDto, Limite>()
                .ForMember(dest => dest.Categoria, opt => opt.Ignore());

            CreateMap<Lancamento, LancamentoDto>()
                .ForMember(dest => dest.CategoriaNome, opt => opt.MapFrom(src => src.Categoria.Nome))
                .ForMember(dest => dest.CartaoNome, opt => opt.MapFrom(src => src.Cartao.Nome))
                .ForMember(dest => dest.ContaNome, opt => opt.MapFrom(src => src.Conta.Nome));
        }
    }
}