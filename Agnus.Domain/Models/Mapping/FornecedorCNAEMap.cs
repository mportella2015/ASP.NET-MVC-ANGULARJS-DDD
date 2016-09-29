using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Conspiracao.Domain.Models.Mapping
{
    public class FornecedorCNAEMap : EntityTypeConfiguration<FornecedorCNAE>
    {
        public FornecedorCNAEMap()
        {
            // Primary Key
            this.HasKey(t => t.NumIdFornecedorCNAE);

            // Properties
            this.Property(t => t.NumIdFornecedorCNAE)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("FornecedorCNAE");
            this.Property(t => t.NumIdFornecedorCNAE).HasColumnName("NumIdFornecedorCNAE");
            this.Property(t => t.CodCNAE).HasColumnName("CodCNAE");
            this.Property(t => t.NumIdFornecedor).HasColumnName("NumIdFornecedor");
            this.Property(t => t.IndPrincipal).HasColumnName("IndPrincipal");

            // Relationships
            this.HasRequired(t => t.ConteudoTabelaDominio)
                .WithMany(t => t.FornecedorCNAEs)
                .HasForeignKey(d => d.CodCNAE);
            this.HasRequired(t => t.Fornecedor)
                .WithMany(t => t.FornecedorCNAEs)
                .HasForeignKey(d => d.NumIdFornecedor);

        }
    }
}
