using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyAPI.Models.Entities;

namespace MoneyAPI.Data.Map
{
    public class LancamentoMap : BaseMap<Lancamento>
    {
        public LancamentoMap() : base("lancamento")
        { }

        public override void Configure(EntityTypeBuilder<Lancamento> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Tipo)
                .HasColumnName("TIPO")
                .HasColumnType("char(7)")
                .IsRequired()
                .IsFixedLength();

            builder.Property(x => x.Valor)
                .HasColumnName("VALOR")
                .HasColumnType("decimal(9,2)")
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasColumnName("DESCRICAO")
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(x => x.Data)
                .HasColumnName("DATA")
                .HasColumnType("date")
                .IsRequired();

            builder.Property(x => x.Observacao)
                .HasColumnName("OBSERVACAO")
                .HasColumnType("varchar(1000)")
                .IsRequired(false);

            builder.Property(x => x.Fixo)
                .HasColumnName("FIXO")
                .HasColumnType("tinyint(1)")
                .IsRequired();

            builder.Property(x => x.PreLancamento)
                .HasColumnName("PRE_LANCAMENTO")
                .HasColumnType("tinyint(1)")
                .IsRequired();

            //relacionamentos

            builder.Property(x => x.CategoriaId)
                .HasColumnName("CATEGORIA_ID")
                .HasColumnType("int unsigned")
                .IsRequired();

            builder.HasOne(l => l.Categoria)
                .WithMany(c => c.Lancamentos)
                .HasForeignKey(c => c.CategoriaId)
                .HasConstraintName("fk_lancamento_categoria");

            builder.HasIndex(x => x.CategoriaId)
                .HasDatabaseName("idx_fk_lancamento_categoria");

            builder.Property(x => x.ContaId)
                .HasColumnName("CONTA_ID")
                .HasColumnType("int unsigned")
                .IsRequired();

            builder.HasOne(l => l.Conta)
                .WithMany(c => c.Lancamentos)
                .HasForeignKey(l => l.ContaId)
                .HasConstraintName("fk_lancamento_conta");

            builder.HasIndex(x => x.ContaId)
                .HasDatabaseName("idx_fk_lancamento_conta");

            builder.Property(x => x.UsuarioId)
                .HasColumnName("USUARIO_ID")
                .HasColumnType("int unsigned")
                .IsRequired();

            builder.HasOne(l => l.Usuario)
                .WithMany(u => u.Lancamentos)
                .HasForeignKey(l => l.UsuarioId)
                .HasConstraintName("fk_lancamento_usuario");

            builder.HasIndex(x => x.UsuarioId)
                .HasDatabaseName("idx_fk_lancamento_usuario");

            builder.Property(x => x.CartaoId)
                .HasColumnName("CARTAO_ID")
                .HasColumnType("int unsigned")
                .IsRequired(false);

            builder.HasOne(l => l.Cartao)
                .WithMany(c => c.Lancamentos)
                .HasForeignKey(l => l.CartaoId)
                .HasConstraintName("fk_lancamento_cartao")
                .IsRequired(false);

            builder.HasIndex(x => x.CartaoId)
                .HasDatabaseName("idx_fk_lancamento_cartao");

            builder.Property(x => x.ContaDestinoId)
                .HasColumnName("CONTA_DESTINO_ID")
                .HasColumnType("int unsigned")
                .IsRequired(false);

            builder.HasOne(l => l.ContaDestino)
                .WithMany()
                .HasForeignKey(l => l.ContaDestinoId)
                .HasConstraintName("fk_lancamento_conta_destino")
                .IsRequired(false);

            builder.HasIndex(x => x.ContaDestinoId)
                .HasDatabaseName("idx_fk_lancamento_conta_destino");
        }
    }
}