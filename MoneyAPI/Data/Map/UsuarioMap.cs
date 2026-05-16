using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyAPI.Models.Entities;

namespace MoneyAPI.Data.Map
{
    public class UsuarioMap : BaseMap<Usuario>
    {
        public UsuarioMap() : base("usuario")
        { }

        public override void Configure(EntityTypeBuilder<Usuario> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Nome)
                .HasColumnName("NOME")
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(x => x.NomeUsuario)
                .HasColumnName("USUARIO")
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(x => x.Senha)
                .HasColumnName("SENHA")
                .HasColumnType("char(60)")
                .IsRequired()
                .IsFixedLength();

            builder.HasIndex(x => x.NomeUsuario)
                .IsUnique()
                .HasDatabaseName("usuario_unique");
        }
    }
}