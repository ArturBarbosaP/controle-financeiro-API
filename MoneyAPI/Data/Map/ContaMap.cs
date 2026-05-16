using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyAPI.Models.Entities;

namespace MoneyAPI.Data.Map
{
    public class ContaMap : BaseMap<Conta>
    {
        public ContaMap() : base("conta")
        { }

        public override void Configure(EntityTypeBuilder<Conta> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Nome)
                .HasColumnName("NOME")
                .HasColumnType("varchar(80)")
                .IsRequired();

            builder.Property(x => x.Saldo)
                .HasColumnName("SALDO")
                .HasColumnType("decimal(9,2)")
                .IsRequired()
                .HasDefaultValueSql("0.00");

            //relacionamentos

            builder.Property(x => x.UsuarioId)
                .HasColumnName("USUARIO_ID")
                .HasColumnType("int unsigned")
                .IsRequired();

            builder.HasOne(c => c.Usuario)
                .WithMany(u => u.Contas)
                .HasForeignKey(c => c.UsuarioId)
                .HasConstraintName("fk_conta_usuario");

            builder.HasIndex(c => c.UsuarioId)
                .HasDatabaseName("idx_fk_conta_usuario");
        }
    }
}