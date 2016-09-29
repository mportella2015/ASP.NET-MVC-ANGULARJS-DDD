using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class FornecedorMaterialMap : EntityTypeConfiguration<FornecedorMaterial>
    {
        public FornecedorMaterialMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdFornecedorMaterial);

            // Properties
            this.Property(t => t.NumIdFornecedorMaterial)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TxtObservacao)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("FornecedorMaterial");
            this.Property(t => t.NumIdFornecedorMaterial).HasColumnName("NumIdFornecedorMaterial");
            this.Property(t => t.NumIdFornecedor).HasColumnName("NumIdFornecedor");
            this.Property(t => t.NumIdTipoMaterial).HasColumnName("NumIdTipoMaterial");
            this.Property(t => t.CodUnidadeMedida).HasColumnName("CodUnidadeMedida");
            this.Property(t => t.ValorUnitario).HasColumnName("ValorUnitario");
            this.Property(t => t.TxtObservacao).HasColumnName("TxtObservacao");
            this.Property(t => t.CodPrazoPagamento).HasColumnName("CodPrazoPagamento");

            // Relationships
            this.HasOptional(t => t.ConteudoTabelaDominio)
                .WithMany(t => t.FornecedorMaterials)
                .HasForeignKey(d => d.CodUnidadeMedida);
            this.HasRequired(t => t.Fornecedor)
                .WithMany(t => t.FornecedorMaterials)
                .HasForeignKey(d => d.NumIdFornecedor);
            this.HasRequired(t => t.TipoMaterial)
                .WithMany(t => t.FornecedorMaterials)
                .HasForeignKey(d => d.NumIdTipoMaterial);

        }
    }
}
