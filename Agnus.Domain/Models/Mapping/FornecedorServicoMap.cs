using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class FornecedorServicoMap : EntityTypeConfiguration<FornecedorServico>
    {
        public FornecedorServicoMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdFornecedorServico);

            // Properties
            this.Property(t => t.NumIdFornecedorServico)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CodPrazoPagamento)
                .HasMaxLength(10);

            this.Property(t => t.TxtObservacao)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("FornecedorServico");
            this.Property(t => t.NumIdFornecedorServico).HasColumnName("NumIdFornecedorServico");
            this.Property(t => t.NumIdFornecedor).HasColumnName("NumIdFornecedor");
            this.Property(t => t.NumIdTipoServico).HasColumnName("NumIdTipoServico");
            this.Property(t => t.ValorUnitario).HasColumnName("ValorUnitario");
            this.Property(t => t.CodUnidadeMedida).HasColumnName("CodUnidadeMedida");
            this.Property(t => t.CodPrazoPagamento).HasColumnName("CodPrazoPagamento");
            this.Property(t => t.TxtObservacao).HasColumnName("TxtObservacao");

            // Relationships
            this.HasOptional(t => t.ConteudoTabelaDominio)
                .WithMany(t => t.FornecedorServicoes)
                .HasForeignKey(d => d.CodUnidadeMedida);
            this.HasRequired(t => t.Fornecedor)
                .WithMany(t => t.FornecedorServicoes)
                .HasForeignKey(d => d.NumIdFornecedor);
            this.HasOptional(t => t.TipoServico)
                .WithMany(t => t.FornecedorServicoes)
                .HasForeignKey(d => d.NumIdTipoServico);

        }
    }
}
