using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyAPI.Models.Entities;

namespace MoneyAPI.Data.Map
{
    public class LimiteMap : BaseMap<Limite>
    {
        public LimiteMap() : base("limite")
        { }

        public override void Configure(EntityTypeBuilder<Limite> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.ValorLimite)
                .HasColumnName("VALOR_LIMITE")
                .HasColumnType("decimal(9,2)")
                .IsRequired()
                .HasDefaultValueSql("0.00");

            //relacionamentos

            builder.Property(x => x.CategoriaId)
                .HasColumnName("CATEGORIA_ID")
                .HasColumnType("int unsigned")
                .IsRequired();

            builder.HasOne(l => l.Categoria)
                .WithOne(c => c.Limite)
                .HasForeignKey<Limite>(l => l.CategoriaId)
                .HasConstraintName("fk_limite_categoria");

            builder.HasIndex(x => x.CategoriaId)
                .HasDatabaseName("idx_fk_limite_categoria");

            builder.HasIndex(x => x.CategoriaId)
                .IsUnique()
                .HasDatabaseName("categoria_id_unique");
        }
    }
}