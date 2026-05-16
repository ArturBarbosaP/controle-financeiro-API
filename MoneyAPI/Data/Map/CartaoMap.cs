using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyAPI.Models.Entities;

namespace MoneyAPI.Data.Map
{
    public class CartaoMap : BaseMap<Cartao>
    {
        public CartaoMap() : base("cartao")
        { }

        public override void Configure(EntityTypeBuilder<Cartao> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Nome)
                .HasColumnName("NOME")
                .HasColumnType("varchar(80)")
                .IsRequired();

            builder.Property(x => x.DataFechamento)
                .HasColumnName("DATA_FECHAMENTO")
                .HasColumnType("date")
                .IsRequired();

            builder.Property(x => x.DataVencimento)
                .HasColumnName("DATA_VENCIMENTO")
                .HasColumnType("date")
                .IsRequired();

            builder.Property(x => x.Limite)
                .HasColumnName("LIMITE")
                .HasColumnType("decimal(9,2)")
                .IsRequired();

            builder.Property(x => x.LimiteDisponivel)
                .HasColumnName("LIMITE_DISPONIVEL")
                .HasColumnType("decimal(9,2)")
                .IsRequired();

            builder.Property(x => x.ValorParcelado)
                .HasColumnName("VALOR_PARCELADO")
                .HasColumnType("decimal(9,2)")
                .IsRequired();

            //relacionamentos

            builder.Property(x => x.ContaId)
                .HasColumnName("CONTA_ID")
                .HasColumnType("int unsigned")
                .IsRequired();

            builder.HasOne(ca => ca.Conta)
                .WithMany(co => co.Cartoes)
                .HasForeignKey(ca => ca.ContaId)
                .HasConstraintName("fk_cartao_conta");

            builder.HasIndex(x => x.ContaId)
                .HasDatabaseName("idx_fk_cartao_conta");
        }
    }
}