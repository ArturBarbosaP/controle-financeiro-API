using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyAPI.Models.Entities;

namespace MoneyAPI.Data.Map
{
    public class CategoriaMap : BaseMap<Categoria>
    {
        public CategoriaMap() : base("categoria")
        { }

        public override void Configure(EntityTypeBuilder<Categoria> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Nome)
                .HasColumnName("NOME")
                .HasColumnType("varchar(80)")
                .IsRequired();

            builder.Property(x => x.Tipo)
                .HasColumnName("TIPO")
                .HasColumnType("char(7)")
                .IsRequired()
                .IsFixedLength();

            builder.Property(x => x.Cor)
                .HasColumnName("COR")
                .HasColumnType("char(7)")
                .IsRequired()
                .IsFixedLength();

            //relacionamentos

            builder.Property(x => x.UsuarioId)
                .HasColumnName("USUARIO_ID")
                .HasColumnType("int unsigned")
                .IsRequired();

            builder.HasOne(c => c.Usuario)
                .WithMany(u => u.Categorias)
                .HasForeignKey(c => c.UsuarioId)
                .HasConstraintName("fk_categoria_usuario");

            builder.HasIndex(c => c.UsuarioId)
            .HasDatabaseName("idx_fk_categoria_usuario");
        }
    }
}