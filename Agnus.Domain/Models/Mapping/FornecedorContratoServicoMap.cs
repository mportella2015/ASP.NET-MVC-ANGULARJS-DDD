using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class FornecedorContratoServicoMap : EntityTypeConfiguration<FornecedorContratoServico>
    {
        public FornecedorContratoServicoMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdFornecedorContratoServico);

            // Properties
            this.Property(t => t.NumIdFornecedorContratoServico)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("FornecedorContratoServico");
            this.Property(t => t.NumIdFornecedorContratoServico).HasColumnName("NumIdFornecedorContratoServico");
            this.Property(t => t.NumIdFornecedorContrato).HasColumnName("NumIdFornecedorContrato");
            this.Property(t => t.NumIdTipoServico).HasColumnName("NumIdTipoServico");
            this.Property(t => t.ValorUnitario).HasColumnName("ValorUnitario");
            this.Property(t => t.CodUnidadeMedida).HasColumnName("CodUnidadeMedida");
            this.Property(t => t.CodPrazoPagamento).HasColumnName("CodPrazoPagamento");

            // Relationships
            this.HasOptional(t => t.ConteudoTabelaDominio)
                .WithMany(t => t.FornecedorContratoServicoes)
                .HasForeignKey(d => d.CodUnidadeMedida);
            this.HasRequired(t => t.FornecedorContrato)
                .WithMany(t => t.FornecedorContratoServicoes)
                .HasForeignKey(d => d.NumIdFornecedorContrato);
            this.HasRequired(t => t.TipoServico)
                .WithMany(t => t.FornecedorContratoServicoes)
                .HasForeignKey(d => d.NumIdTipoServico);

        }
    }
}
