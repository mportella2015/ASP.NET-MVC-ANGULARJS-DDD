using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class FornecedorPendenciaMap : EntityTypeConfiguration<FornecedorPendencia>
    {
        public FornecedorPendenciaMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdFornecedorPendencia);

            // Properties
            this.Property(t => t.NumIdFornecedorPendencia)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TxtPendencia)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("FornecedorPendencia");
            this.Property(t => t.NumIdFornecedorPendencia).HasColumnName("NumIdFornecedorPendencia");
            this.Property(t => t.NumIdFornecedor).HasColumnName("NumIdFornecedor");
            this.Property(t => t.DataPendencia).HasColumnName("DataPendencia");
            this.Property(t => t.DataResolucao).HasColumnName("DataResolucao");
            this.Property(t => t.CodTipoPendencia).HasColumnName("CodTipoPendencia");
            this.Property(t => t.TxtPendencia).HasColumnName("TxtPendencia");

            // Relationships
            this.HasRequired(t => t.ConteudoTabelaDominio)
                .WithMany(t => t.FornecedorPendencias)
                .HasForeignKey(d => d.CodTipoPendencia);
            this.HasOptional(t => t.Fornecedor)
                .WithMany(t => t.FornecedorPendencias)
                .HasForeignKey(d => d.NumIdFornecedor);

        }
    }
}
